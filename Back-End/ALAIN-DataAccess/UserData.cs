using ALAIN_DataAccess.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ALAIN_DataAccess
{
    public static class UserData
    {
        public static Dictionary<string,int>? GetUserStatistics()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select 
                                   count(case when IsActive=1 then 1 end) as ActiveUsers,
                                   count(case when IsActive=0 then 0 end) as InactiveUsers
                               from Users;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            Dictionary<string, int> statistics = new Dictionary<string, int>(2);
                            string s = "ActiveUsers";
                            statistics.Add(s, reader.GetInt32(s));

                            s = "InactiveUsers";
                            statistics.Add(s, reader.GetInt32(s));

                            return statistics;
                        }
                        return null;
                    }
                }
            }
        }
        public static bool BanUser(string UserName)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"update Users set IsActive=0 where Username=@UserName;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public static bool UnbanUser(string UserName)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"update Users set IsActive=1 where Username=@UserName;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public static List<User_DTO> GetAllUsers()
        {
            List<User_DTO>users = new List<User_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Users.UserID,Users.Username,Users.FirstName,Users.LastName,
                                Users.Email,Roles.RoleName as Role,Users.IsActive
                                from Users join Roles on Users.RoleID=Roles.RoleID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User_DTO(reader.GetInt32("UserID"), reader.GetString("Username"), 
                                reader.GetString("FirstName"), reader.GetString("LastName"), 
                                reader.GetString("Email"), reader.GetString("Role"),
                                reader.GetBoolean("IsActive")));
                        }
                    }
                }
            }
            return users;
        }
        public static int? AddNewUser(User_DTO user)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"insert into Users(FirstName,LastName,Username,PasswordHash,Email,RoleID)values
                                (@FirstName,@LastName,@Username,@Password,@Email,
                                (select RoleID from Roles where RoleName=@Role)); select SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Username", user.UserName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Role", user.Role);

                    object? result = command.ExecuteScalar();
                    if (result == DBNull.Value || result == null)
                        return null;

                    return Convert.ToInt32(result);
                }
            }
        }
        public static bool EditUser(User_DTO user)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"UPDATE Users SET FirstName=@FirstName,LastName=@LastName,Username=@Username,
                               PasswordHash=@Password,Email=@Email,RoleID=(select RoleID from Roles where RoleName=@Role)
                               WHERE UserID=@UserID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", user.Id);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Username", user.UserName);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Role", user.Role);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public static bool CheckIfUsernameExist(string Username)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select iif(exists(select 1 from Users where Username=@User),1,0);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@User", Username);
                    return Convert.ToBoolean(command.ExecuteScalar());
                }
            }
        }
        public static User_DTO? GetUserInfoByUsername(string Username)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Users.UserID,Users.Username,Users.FirstName,Users.LastName,Users.PasswordHash,
                                Users.Email,Roles.RoleName,Users.IsActive from Users join Roles on Users.RoleID=Roles.RoleID
                                where Username=@User and IsActive=1;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@User", Username);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return new User_DTO(reader.GetInt32("UserID"), reader.GetString("Username"),
                                reader.GetString("FirstName"), reader.GetString("LastName"), reader.GetString("Email"),
                                reader.GetString("RoleName"), reader.GetString("PasswordHash"), reader.GetBoolean("IsActive"));
                        }
                        return null;
                    }
                }
            }
        }
        public static User_DTO? GetUserInfoByUserID(int UserID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                string query = @"select Users.UserID,Users.Username,Users.FirstName,Users.LastName,Users.PasswordHash,
                                Users.Email,Roles.RoleName,Users.IsActive from Users join Roles on Users.RoleID=Roles.RoleID
                                where UserID=@UserID and IsActive=1;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User_DTO(reader.GetInt32("UserID"), reader.GetString("Username"),
                                reader.GetString("FirstName"), reader.GetString("LastName"), reader.GetString("Email"),
                                reader.GetString("RoleName"), reader.GetString("PasswordHash"), reader.GetBoolean("IsActive"));
                        }
                        return null;
                    }
                }
            }
        }
        public static User_DTO? GetUserByRefreshTokenHash(string hash)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                string query = @"select Users.UserID,Users.Username,Users.PasswordHash,
                                Users.Email,Roles.RoleName,Users.IsActive,RefreshTokenHash,RefreshTokenExpiresAt,
                                RefreshTokenRevokedAt FROM Users JOIN Roles on Roles.RoleID=Users.RoleID
                                WHERE RefreshTokenHash = @Hash and IsActive=1;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Hash", SqlDbType.NVarChar, 64).Value = hash;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return new User_DTO
                        {
                            Id = Convert.ToInt32(reader["UserID"]),
                            UserName = reader["UserName"].ToString()!,
                            Password = reader["PasswordHash"].ToString()!,
                            Role = reader["RoleName"].ToString()!,
                            IsActive = Convert.ToBoolean(reader["IsActive"]),

                            RefreshTokenHash = reader["RefreshTokenHash"].ToString()!,

                            RefreshTokenExpiresAt =
                                Convert.ToDateTime(reader["RefreshTokenExpiresAt"]),

                            RefreshTokenRevokedAt =
                                reader["RefreshTokenRevokedAt"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(reader["RefreshTokenRevokedAt"])
                        };
                    }
                }
            }
        }
        public static bool UpdateRefreshToken(int userId, string hash, DateTime expiresAt)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                string query = @"UPDATE Users SET
                RefreshTokenHash = @Hash,
                RefreshTokenExpiresAt = @ExpiresAt,
                RefreshTokenRevokedAt = NULL
                WHERE UserID = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = userId;

                    command.Parameters.Add("@Hash", SqlDbType.NVarChar, 64)
                        .Value = hash;

                    command.Parameters.Add("@ExpiresAt", SqlDbType.DateTime2)
                        .Value = expiresAt;

                    connection.Open();

                    return command.ExecuteNonQuery() == 1;
                }
            }
        }
        public static bool RotateRefreshToken(int userId, string oldHash, string newHash, DateTime expiresAt)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                string query = @"UPDATE Users SET
                RefreshTokenHash = @NewHash,
                RefreshTokenExpiresAt = @ExpiresAt
                WHERE
                UserID = @Id
                AND RefreshTokenHash = @OldHash
                AND RefreshTokenRevokedAt IS NULL
                AND RefreshTokenExpiresAt > SYSUTCDATETIME()";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = userId;

                    command.Parameters.Add("@OldHash", SqlDbType.NVarChar, 64)
                        .Value = oldHash;

                    command.Parameters.Add("@NewHash", SqlDbType.NVarChar, 64)
                        .Value = newHash;

                    command.Parameters.Add("@ExpiresAt", SqlDbType.DateTime2)
                        .Value = expiresAt;

                    connection.Open();

                    return command.ExecuteNonQuery() == 1;
                }
            }
        }
        public static bool RevokeRefreshToken(int userId, string hash, DateTime revokedAt)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                string query = @"
                UPDATE Users
                SET RefreshTokenRevokedAt = @RevokedAt
                WHERE
                UserID = @Id
                AND RefreshTokenHash = @Hash
                AND RefreshTokenRevokedAt IS NULL";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = userId;

                    command.Parameters.Add("@Hash", SqlDbType.NVarChar, 64)
                        .Value = hash;

                    command.Parameters.Add("@RevokedAt", SqlDbType.DateTime2)
                        .Value = revokedAt;

                    connection.Open();

                    return command.ExecuteNonQuery() == 1;
                }
            }
        }
    }
}