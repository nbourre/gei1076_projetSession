using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestionnaire
{
    class GestionnaireBUS
    {
        private static GestionnaireDAO gestionnaireDAO = new GestionnaireDAO();

        public static List­<LocalModel> obtenirContactParNumero(string numero)
        {
            List<LocalModel> locaux = new List<LocalModel>();

            DataTable dt = new DataTable();

            dt = gestionnaireDAO.rechercheLocauxParNumero(numero);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    LocalModel local = new LocalModel();

                    local.id = Int32.Parse(row["id"].ToString());
                    local.numero = row["numero"].ToString();
                    local.description = row["description"].ToString();

                    locaux.Add(local);
                }
            }

            return locaux;
        }
    }
}
