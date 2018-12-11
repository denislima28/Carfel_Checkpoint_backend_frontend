using System;

namespace carfel_checkpoint.web.Models
{
    public class ComentarioModel
    {
        private object texto;

        public int IDcomentario {get; set; }

        public int IDusuario { get; set; }

        public string Texto { get; set; }

        public DateTime DataPostagem {get; set;}

        public string Status {get; set;}  //Aprovado ou pendente. Comentários reprovados são excluídos

        
        public ComentarioModel(int idcomentario, int idusuario, string texto, DateTime dataPostagem, string status)
        {
            IDcomentario = idcomentario;
            IDusuario = idusuario;
            Texto = texto;
            DataPostagem = dataPostagem;
            Status = status;
        } //O this significa que está utilizando os parâmetros desta classe mesmo.

        //O programa pediu para criar o construtor quando eu estava criando o objeto para ser carregado no CSV em ComentarioRepositorio.CSV

        public ComentarioModel(string texto, int idUsuario)
        {
            this.Texto = texto;
            this.IDusuario = idUsuario; //associa o comentário a id do usuário que o criou
            this.DataPostagem = DateTime.Now; //Coloca o horário 
        }//O programa pediu para eu criar o construtor quando eu estava criando o Envio_de_comentario no ComentarioController.





    }
}