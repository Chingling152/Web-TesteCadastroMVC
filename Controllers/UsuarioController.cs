using System.IO;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ProjetoFinancas.Models;

namespace Senai.web.tarefas.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult Cadastrar()=> View();
        
        [HttpPost]
        public ActionResult Cadastrar(IFormCollection form){

            Usuario usuario = new Usuario(){
                ID = System.IO.File.ReadAllLines("Databases/Usuario.csv").Length+1,
                Nome = form["Nome"],
                Email = form["Email"],
                Senha = form["Senha"],
                Tipo = form["Tipo"],
                DataNascimento = DateTime.Parse(form["DataNascimento"])
            };  
            string mensagem = tudoOK(usuario);

            if(mensagem == $"Usuario {usuario.Nome} cadastrado com sucesso"){
                using(StreamWriter sw = new StreamWriter("Databases/Usuario.csv",true)){
                    sw.WriteLine($"{usuario.Nome};{usuario.Email};{usuario.Senha};{usuario.Tipo};{usuario.DataNascimento}");
                }
                return RedirectToAction("Login","Usuario");
            }else{
                ViewBag.Erro = mensagem;
                return View();
            }
        }

        public ActionResult Login()=> View();

        public ActionResult Login(IFormCollection form){
            Usuario usuario = new Usuario(){
                Email = form["Email"],
                Senha = form["Senha"]
            };

            using(StreamReader leitor = new StreamReader("Databases/Usuario.csv")){
                while(!leitor.EndOfStream){
                    string[] info = leitor.ReadLine().Split(';');

                    if(usuario.Email == info[2]  && usuario.Senha == info[3]){
                        HttpContext.Session.SetString("ID",usuario.ID.ToString());
                        return RedirectToAction("Criar","Tarefa");
                    }
                }
            }

            return RedirectToAction("Logar","Usuario");
        }  

        private string tudoOK(Usuario user){
            if(string.IsNullOrEmpty(user.Nome)){
                return "Nome invalido!!";
            }
            if(string.IsNullOrEmpty(user.Senha)){
                return "Senha invalida!!";
            }
            return $"Usuario {user.Nome} cadastrado com sucesso";
        }
    }

}