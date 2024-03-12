using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Biblioteca.Models
{
    public class UsuarioService
    {
        public List<Usuario> Listar()
        {
            using(BibliotecaContext bc = new BibliotecaContext ())
            {
                return bc.Usuarios.ToList();
            }
        }

        public Usuario Listar(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext ())
            {
                return bc.Usuarios.Find(id);
            }
        }


        public void incluirUsuario(Usuario u)
        {
            using (BibliotecaContext bc = new BibliotecaContext ())
            {
                bc.Add(u);
                bc.SaveChanges();
            }
        }

        public void editarUsuario(Usuario u)
        {
            using (BibliotecaContext bc = new BibliotecaContext ())
            {
                Usuario usuarioBD = bc.Usuarios.Find(u.Id);
                usuarioBD.Login = u.Login;
                usuarioBD.Nome = u.Nome;
                
                if(usuarioBD.Senha != u.Senha)
                {
                    usuarioBD.Senha = Criptografo.TextoCriptografado(u.Senha);
                }
                else
                {
                    usuarioBD.Senha = u.Senha;
                }

                usuarioBD.Tipo = u.Tipo;

                bc.SaveChanges();
            }
        }

        public void excluirUsuario(int id)
        {
            using (BibliotecaContext bc = new BibliotecaContext ())
            {
                bc.Usuarios.Remove (bc.Usuarios.Find(id));
                bc.SaveChanges ();
            }
        }

        public ICollection<Usuario> ListarTodos (int pagina = 1, int tamanho = 5, Filtragem filtro = null)
        {
            using (BibliotecaContext bc = new BibliotecaContext ())
            {
                IQueryable<Usuario> query;
                int pular = (pagina - 1) * tamanho;

                if (filtro != null)
                {
                    switch (filtro.TipoFiltro)
                    {
                        case "Nome":
                            query = bc.Usuarios.Where (u => u.Nome.Contains (filtro.Filtro, StringComparison.CurrentCultureIgnoreCase));
                            break;

                        case "Login":
                            query = bc.Usuarios.Where (u => u.Login.Contains (filtro.Filtro, StringComparison.CurrentCultureIgnoreCase));
                            break;

                        default:
                            query = bc.Usuarios;
                            break;
                    }
                } else {
                    query = bc.Usuarios;
                }
                return query.Skip (pular).Take (tamanho).ToList ();
            }
        }

        public int NumeroDeUsuarios ()
        {
            using (var context = new BibliotecaContext ())
            {
                return context.Usuarios.Count ();
            }
        }
    }
}