using System;
using System.Collections.Generic;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult administrador()
        {
            return View();
        }

        public IActionResult ListaDeUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View(new UsuarioService().Listar());
        }

        public IActionResult RegistrarUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View();
        }

        [HttpPost]
        public IActionResult RegistrarUsuarios(Usuario novoUser)
        {
            novoUser.Senha = Criptografo.TextoCriptografado(novoUser.Senha);

            new UsuarioService().incluirUsuario(novoUser);

            return RedirectToAction("ListaDeUsuarios");
        }

        public IActionResult EditarUsuario(int Id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View(new UsuarioService().Listar(Id));
        }

        [HttpPost]
        public IActionResult EditarUsuario(Usuario userEditado)
        {
            new UsuarioService().editarUsuario(userEditado);
            return RedirectToAction("ListaDeUsuarios");
        }

        public IActionResult ExcluirUsuario(int Id)
        {
            new UsuarioService().excluirUsuario(Id);
            return RedirectToAction("ListaDeUsuarios");
        }

        public IActionResult Listagem (string tipoFiltro, string filtro, int p = 1)
        {
            Autenticacao.CheckLogin(this);
            Filtragem objFiltro = null;
            if (!string.IsNullOrEmpty (filtro))
            {
                objFiltro = new Filtragem();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            int quantidadePorPagina = 5;
            UsuarioService usuarioService = new UsuarioService();
            int totalDeRegistros = usuarioService.NumeroDeUsuarios();
            ICollection<Usuario> lista = usuarioService.ListarTodos (p, quantidadePorPagina, objFiltro);
            ViewData["NroPaginas"] = (int) Math.Ceiling ((double) totalDeRegistros / quantidadePorPagina);
            return View(lista);
        }
    }
}