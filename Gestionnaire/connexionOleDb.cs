using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Diagnostics;
using System;
using System.Reflection;

namespace Gestionnaire
{

    class connexionOleDb
    {
        public enum CRUDType { Create, Read, Update, Delete};

        OleDbDataAdapter adapteur;
        OleDbConnection conn;

        public connexionOleDb ()
        {
            adapteur = new OleDbDataAdapter();
            conn = new OleDbConnection(ConfigurationManager.AppSettings["connString"]);
        }

        /// <summary>
        /// Fonction permettant d'ouvrir la connexion et de la retourner
        /// </summary>
        /// <returns>La connexion</returns>
        private OleDbConnection ouvrirConnexion()
        {
            if (conn.State == ConnectionState.Open||
                conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }

            return conn;
        }

        /// <summary>
        /// Méthode helper permettant d'afficher un message d'erreur dans la console
        /// </summary>
        /// <param name="_message">Le message à afficher</param>
        private void writeError(string _message)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            Console.Write("Erreur - " + methodBase.Name + " - " + _message);
        }


        public DataTable executeSelectQuery(string _query, OleDbParameter[] sqlParameter)
        {
            OleDbCommand commande = new OleDbCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();
            try
            {
                commande.Connection = ouvrirConnexion();
                commande.CommandText = _query;
                commande.Parameters.AddRange(sqlParameter);
                commande.ExecuteNonQuery();
                adapteur.SelectCommand = commande;
                adapteur.Fill(ds);
                dataTable = ds.Tables[0];
            }
            catch (OleDbException e)
            {
                writeError("Requête : " + _query + "\nSqlException : " + e.StackTrace.ToString());
                return null;
            }

            return dataTable;
        }

        public bool executeInsertQuery(string _query, OleDbParameter[] sqlParameters)
        {
            return executeNonQuery(_query, sqlParameters, CRUDType.Create);
        }

        public bool executeUpdateQuery(string _query, OleDbParameter[] sqlParameters)
        {
            return executeNonQuery(_query, sqlParameters, CRUDType.Update);
        }

        public bool executeDeleteQuery(string _query, OleDbParameter[] sqlParameters)
        {
            return executeNonQuery(_query, sqlParameters, CRUDType.Delete);
        }

        private bool executeNonQuery (string _query, OleDbParameter[] sqlParameters, CRUDType crud)
        {

            OleDbCommand commande = new OleDbCommand();
            try
            {
                commande.Connection = ouvrirConnexion();
                commande.CommandText = _query;
                commande.Parameters.AddRange(sqlParameters);

                switch (crud)
                {
                    case CRUDType.Create:
                        adapteur.InsertCommand = commande;
                        break;
                    case CRUDType.Delete:
                        adapteur.DeleteCommand = commande;
                        break;
                    case CRUDType.Update:
                        adapteur.UpdateCommand = commande;
                        break;
                    case CRUDType.Read:
                        throw new Exception("La requête SELECT est n'est pas utilisable dans ce contexte.");
                }

                
                commande.ExecuteNonQuery();
            }
            catch (OleDbException e)
            {
                writeError("Requête : " + _query + "\nSqlException : " + e.StackTrace.ToString());
                return false;
            }
            return true;

        }


    }
}
