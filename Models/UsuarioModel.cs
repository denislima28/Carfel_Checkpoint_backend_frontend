using Microsoft.Extensions.Primitives;

namespace carfel_checkpoint.web.Models
{
    public class UsuarioModel
    {
        public int ID { get; set; }

        public string Nome {get; set;}

        public string Email { get; set; }

        public string Senha { get; set; }

        public bool Administrador { get; set; }

        public UsuarioModel(int id, string nome, string email, string senha, bool administrador) //Método construtor
        //O this significa que está utilizando os parâmetros desta classe mesmo.
        {
            this.ID = id;
            this.Nome = nome;
            this.Email = email;
            this.Senha = senha;
            this.Administrador = administrador;
        }

        public UsuarioModel(string nome, string email, string senha, bool administrador)
        {
            this.Nome = nome;
            this.Email = email;
            this.Senha = senha;
            this.Administrador = administrador;
        } //O programa pediu para gerar esse construtor quando o método  public ActionResult Cadastro(IFormCollection form) foi criado no UsuarioController

        public UsuarioModel(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }//O programa pediu esse construtor por causa do Login no UsuarioController.  
    }

    
}