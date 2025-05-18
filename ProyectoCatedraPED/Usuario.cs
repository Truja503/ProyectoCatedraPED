using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDeCatedraPED
{
    class Usuario
    {
        // Atributtes
        int id;
        string fullname;
        string username;
        string password;

        // Set&Get
        public int ID
        {
            get { return this.id; }
        }

        public string Fullname
        {
            get { return fullname; }
            set { fullname = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        // Methods
        public Usuario(int id, string fullname, string username)
        {
            this.id = id;
            this.fullname = fullname;
            this.username = username;
        }

        public Usuario(string fullname, string username)
        {
            this.fullname = fullname;
            this.username = username;
        }


        // Agregar
        public bool AgregarUsuario()
        {
            return false;
            // return true si se creo con exito
        }

        // Modificar 
        public bool ModificarUsuario()
        {
            return false;
            // return true si se modifico con exito
        }


        // Eliminar
        public bool EliminarUsuario()
        {
            return false;
            // return true si se elimino con exito
        }
    }
}
