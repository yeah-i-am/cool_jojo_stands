/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace cool_jojo_stands.Utils
{
    public class BonusManager
    {
        private static bool SPRequiemBonus = false;
        private static string
            Host = "212.109.219.8",
            DB = "pass",
            User = "GayLord",
            Pass = "YoYoKlassI239Toje";
        private static int Port = 3306;

        private static MySqlConnection BonusDB = null;

        private static bool DBConnected = false;

        public static bool StarPlatinumReguiemBonus
        {
            get { return SPRequiemBonus; }
        }

        public static bool Connected
        {
            get { return DBConnected; }
        }

        public static MySqlConnection GetDBConnection(string host, int port, string database, string username, string password)
        {
            // Connection String.
            String connString = "Server=" + host + ";Database=" + database
                + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection connection = new MySqlConnection(connString);

            return connection;
        }

        public static void Init()
        {
            BonusDB = GetDBConnection(Host, Port, DB, User, Pass);

            if (!BonusDB.Ping())
                try
                {
                    BonusDB.Open();
                }
                catch (Exception e)
                {
                    cool_jojo_stands.mod.Logger.Warn("Can't connect to DB: " + e.Message);
                    return;
                }
            else
                cool_jojo_stands.mod.Logger.Warn("Can't connect to DB: High ping");


            DBConnected = true;
        }

        public static void UnLoad()
        {
            BonusDB.Close();
            BonusDB.ClearAllPoolsAsync();
            BonusDB.Dispose();

            DBConnected = false;
        }

        public static bool UpdateBonuses()
        {
            if (!cool_jojo_stands.usingSteam || cool_jojo_stands.SteamId == 0)
                return false;

            string Command = "SELECT * FROM Bonuses WHERE steam_id = " + cool_jojo_stands.SteamId.ToString();

            MySqlCommand cmd = BonusDB.CreateCommand();

            cmd.CommandText = Command;

            int numOfBonuses = 0;

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        switch (reader.GetString(3))
                        {
                            case "SPR":
                                SPRequiemBonus = true;
                                numOfBonuses++;
                                break;
                        }
                    }
                }
                else return false;
            }

            if (numOfBonuses > 0)
                return true;

            return false;
        }

        private static bool ReserveBonus( int id )
        {
            string Command = "UPDATE Bonuses SET steam_id = " + cool_jojo_stands.SteamId.ToString()
                + " WHERE id = " + id.ToString();

            MySqlCommand cmd = BonusDB.CreateCommand();

            cmd.CommandText = Command;

            try
            {
                int rowCount = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                cool_jojo_stands.mod.Logger.Warn("DB error: " + e.Message);
                return false;
            }

            return true;
        }

        public static bool ActivateBonus( string Key )
        {
            if (!cool_jojo_stands.usingSteam || cool_jojo_stands.SteamId == 0)
                return false;

            string Command = "SELECT * FROM Bonuses WHERE skey = \'" + Key + "\'";

            MySqlCommand cmd = BonusDB.CreateCommand();

            cmd.CommandText = Command;

            int id = 0;

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    if (Convert.ToUInt64(reader.GetValue(2)) == 0)
                    {
                        switch (reader.GetString(3))
                        {
                            case "SPR":
                                SPRequiemBonus = true;
                                break;
                            default:
                                return false;
                        }

                        id = Convert.ToInt32(reader.GetValue(0));
                    } else return false;
                } else return false;
            }

            if (id != 0)
              return ReserveBonus(id);

            return false;
        }
    }
}
*/