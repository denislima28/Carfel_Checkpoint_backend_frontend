using carfel_checkpoint.web.Interfaces;
using carfel_checkpoint.web.Models;
using carfel_checkpoint.web.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace carfel_checkpoint.web.Controllers {
    public class UsuarioController : Controller //Controller é uma classe do visual code.
    {
        public IUsuario UsuarioRepositorio {get; set;} //Feito para não precisar instanciar isso toda hora. Antes, o objeto UsuarioRepositorio era criado várias vezes.

         //Construtor da classe
        public UsuarioController() 
        {
            UsuarioRepositorio = new UsuarioRepositorioCSV();          
        }
        public ActionResult Index () // Faz aparecer a página Home.
        {
            return View ();
        }

        public ActionResult Quem_somos () // Faz aparecer a página Quem Somos.
        {
            return View ();
        }

        public ActionResult FAQ () // Faz aparecer a página FAQ.
        {
            return View ();
        }

        public ActionResult Fale_conosco () // Faz aparecer a página Fale Conosco.
        {
            return View ();
        }

        [HttpGet]
        public ActionResult Cadastro () //Se refere à página "Cadastre-se"
        {
            return View ();
        }

        [HttpPost]
        public ActionResult Cadastro (IFormCollection form) {
            UsuarioModel usuarioModel = new UsuarioModel (nome: form["nome"], email: form["email"], senha: form["senha"], administrador: bool.Parse (form["administrador"]));
            //A ordem faria diferença sem o rótulo (ex.: nome:), pois estaria passando por posição (mesma ordem do formulário). Contudo, o rótulo elimina a necessidade de colocar na mesma ordem do formulário.          
            // O id não aparece aqui porque não será digitada pelo usuário
            UsuarioRepositorio.Cadastrar(usuarioModel);

            ViewBag.Mensagem = "Usuário Cadastrado";

            return View ();
        }

        [HttpGet]
        public IActionResult Login () => View ();

        [HttpPost]
        public IActionResult Login (IFormCollection form) 
        {
            //Pega os dados do POST
            UsuarioModel usuario = new UsuarioModel(email: form["email"], senha: form["senha"]); //Funciona sem os rótulos. Com eles, não é preciso colocar em ordem.

            //Verificar se o usuário possui acesso para realizar login
            UsuarioRepositorioCSV usuarioRep = new UsuarioRepositorioCSV();

             UsuarioModel usuarioModel = usuarioRep.BuscarPorEmailESenha(usuario.Email, usuario.Senha);

            if (usuarioModel != null) 
            {
                HttpContext.Session.SetString("idUsuario", usuarioModel.ID.ToString());
                //Se der algo errado, trocar "ID" por "Email".

                ViewBag.Mensagem = "Login realizado com sucesso!";

                return RedirectToAction("Index", "Usuario"); //Direciona para a primeira página depois do login
            }
            else
            {
                ViewBag.Mensagem = "Acesso negado!";
            }

            return View();
        }

         /// <summary>
        /// Lista todos os usuários cadastrados no sistema
        /// </summary>
        /// <returns>A view da listagem de usuário</returns>

        [HttpGet]

        public IActionResult Listar() //passa dados do reposistório para o controller
        {
            UsuarioRepositorioCSV rep = new UsuarioRepositorioCSV();

            //Buscando os dados do repositório e aplicando no viewbag.
            //ViewBag.Usuarios = rep.Listar(); //ViewBag é para quando você quer mostrar um dado simples: número, texto etc...
            ViewData["Usuario"] = rep.Listar(); //Para quando quisermos mandar um objeto. Neste caso, a lista do usuários.

            return View();
        }

         [HttpGet] //Neste caso específico, também poderia ser POST, pois parâmetros GET podem ser usados por meio do HttpPost ou do HttpGet

        public IActionResult Excluir(int id)
        {
           
            UsuarioRepositorio.Excluir(id); //Chama o método de exclusão.
            
            TempData["Mensagem"] = "Usuário excluído"; //O ViewBag e o ViewData só servem para a View na qual estão. Por isso, a mensagem não aparece se houver um redirecionamento para outra página. Assim, é preciso usar o TempData.
            
            return RedirectToAction("Listar"); //O comando manda o programa fazer a lista novamente. O View não funciona aqui, pois para funcionar, seria preciso que o método Excluir chamasse os usários. O Listar já faz isso. Por isso que é melhor usar o Redirect.
        }

    }
}