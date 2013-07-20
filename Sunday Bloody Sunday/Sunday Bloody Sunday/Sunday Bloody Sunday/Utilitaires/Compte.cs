using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sunday_Bloody_Sunday
{
    [Serializable]
    class Compte
    {
        public int campagne1;
        public int campagne2;
        public int campagne3;
        public string pseudo;

        public Compte(string pseudo)
        {
            this.pseudo = pseudo;
            campagne1 = 1;
            campagne2 = 1;
            campagne3 = 1;
        }

        public static void Enregistrer(Compte toSave, string path)//Save/Save.poney
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream flux = null;
            try
            {
                flux = new FileStream(path, FileMode.Create, FileAccess.Write);
                formatter.Serialize(flux, toSave);
                flux.Flush();
            }
            catch { }
            finally
            {
                if (flux != null)
                    flux.Close();
            }
        }

        public static bool Charger(string path, ref Compte poney)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream flux = null;
            try
            {
                flux = new FileStream(path, FileMode.Open, FileAccess.Read);

                poney = (Compte)formatter.Deserialize(flux);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (flux != null)
                    flux.Close();
            }
        }
    }
}
