using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace Gestionnaire
{
    class GestionnaireDAO
    {
        private connexionOleDb conn;

        public GestionnaireDAO()
        {
            conn = new connexionOleDb();
        }

        /// <summary>
        /// Recherche les enregistrements par nom ou prénom
        /// </summary>
        /// <param name="_numero">Terme de la recherche</param>
        /// <returns>Table de résultats</returns>
        public DataTable rechercheLocauxParNumero(string _numero)
        {
            string query = string.Format(
               "SELECT * " +
               "FROM [Locaux] " +
               "WHERE numero LIKE @numero");
            OleDbParameter[] sqlParameters = new OleDbParameter[1];
            sqlParameters[0] = new OleDbParameter("@numero", OleDbType.WChar);
            sqlParameters[0].Value = Convert.ToString(_numero);

            return conn.executeSelectQuery(query, sqlParameters);
        }



    }
}
