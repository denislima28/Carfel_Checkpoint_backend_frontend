using carfel_checkpoint.web.Interfaces;
using carfel_checkpoint.web.Models;
using carfel_checkpoint.web.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace carfel_checkpoint.web.Controllers
{
    public class ComentarioController : Controller //Controller é uma classe do visual code.
    {
        public IComentario ComentarioRepositorio {get; set;}//Feito para não precisar instanciar isso toda hora. Sem isso, o objeto precisaria ser criado várias vezes

        //Construtor da classe
        public ComentarioController()
        {
            ComentarioRepositorio = new ComentarioRepositorioCSV();
        }

        
        // [HttpGet]
        // public ActionResult Envio_de_comentario(){
            
        //     return View();
        // } Isso, sozinho, faz aparecer a página Envio_de_comentário. Com o método abaixo, faz isso e também perimite aparecer comentários listados. Em outra parte do programa, há ajustes para aparecer apenas os comentários aprovados. 
        
        
        [HttpGet]

        public ActionResult Envio_de_comentario(){
            
             ComentarioRepositorioCSV rep = new ComentarioRepositorioCSV();

             ViewData["Comentario"] = rep.Listar_comentario(); //Para quando quisermos mandar um objeto. Neste caso, a lista de comentários.

             return View();
         }


        [HttpPost]
        public ActionResult Envio_de_comentario(IFormCollection form)
        {
            //O if é para saber se usuário está logado. Apenas usuários logados podem fazer comentários.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("idUsuario"))) {   //Se o id existe, é porque o usuário está logado.
                
                return RedirectToAction ("Login","Usuario"); //Se o usuário tentar postar um comentário sem estar logado, a página o levará automaticamente para a página de Login.
            }
            
            ComentarioModel comentarioModel = new ComentarioModel(texto: form["texto"], idUsuario: int.Parse(HttpContext.Session.GetString("idUsuario")));
            //Apenas o texto aparecerá no formulário porque o sistema preencherá as demais informações (como a do Id do usuário).

            ComentarioRepositorio.Enviar_comentario(comentarioModel);

             ComentarioRepositorioCSV rep = new ComentarioRepositorioCSV();

             ViewData["Comentario"] = rep.Listar_comentario(); //Para quando quisermos mandar um objeto. Neste caso, a lista de comentários.

             //Para exibir os comentários aprovados e permitir a postagem de mais comentários, os dois comandos anteriores a este comentário precisam estar aqui e também no método Get.

            ViewBag.Mensagem = "Comentário enviado";

            return View();
        }

         /// <summary>
        /// Lista todos os usuários cadastrados no sistema
        /// </summary>
        /// <returns>A view da listagem de usuário</returns>

        [HttpGet]

        public IActionResult Listar_comentario() //passa dados do reposistório para o controller
        {
            
            //O if é para saber se usuário está logado. Apenas usuários logados podem listar as páginas
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("idUsuario"))) {   //Se o id existe, é porque o usuário está logado.

            //Colocar no comentario.csv se o usuário é administrador ou não. Assim, eu poderei acrescentar o admin na condição acima. A ideia é escrever: Se o usuário estiver deslogado ou se o usuário não for administrador.
                
                return RedirectToAction ("Login","Usuario"); //Se o usuário tentar listar sem estar logado, a página o levará automaticamente para a página de Login.
            }
            
            
            ComentarioRepositorioCSV rep = new ComentarioRepositorioCSV();

            //Buscando os dados do repositório e aplicando no viewbag.
            //ViewBag.Comentarios = rep.Listar(); //ViewBag é para quando você quer mostrar um dado simples: número, texto etc...
            ViewData["Comentario"] = rep.Listar_comentario(); //Para quando quisermos mandar um objeto. Neste caso, a lista de comentários.

            return View();
        }

        [HttpGet]

        public IActionResult Aprovar_comentario(int idcomentario)
        {
            ComentarioRepositorio.Aprovar_comentario(idcomentario); //Chama o método de aprovacao.
            
            TempData["Mensagem"] = "Comentário aprovado";

            return RedirectToAction("Listar_comentario"); //O comando manda o programa fazer a lista novamente. O View não funciona aqui, pois para funcionar, seria preciso que o método Excluir chamasse os comentários. O Listar_comentario já faz isso. Por isso que é melhor usar o Redirect.

        }//Esse método precisa ser melhorado
        
       
         [HttpGet] //Neste caso específico, também poderia ser POST, pois parâmetros GET podem ser usados por meio do HttpPost ou do HttpGet

        public IActionResult Reprovar_Excluir_comentario(int idcomentario)
        {
           
            ComentarioRepositorio.Reprovar_Excluir_comentario(idcomentario); //Chama o método de exclusão.
            
            TempData["Mensagem"] = "Comentário excluído"; //O ViewBag e o ViewData só servem para a View na qual estão. Por isso, a mensagem não aparece se houver um redirecionamento para outra página. Assim, é preciso usar o TempData.
            
            return RedirectToAction("Listar_comentario"); //O comando manda o programa fazer a lista novamente. O View não funciona aqui, pois para funcionar, seria preciso que o método Excluir chamasse os comentários. O Listar_comentario já faz isso. Por isso que é melhor usar o Redirect.
        }

    }
}