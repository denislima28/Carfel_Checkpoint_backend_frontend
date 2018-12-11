using System;
using System.Collections.Generic;
using System.IO;
using carfel_checkpoint.web.Interfaces;
using carfel_checkpoint.web.Models;
using Microsoft.AspNetCore.Http;

namespace carfel_checkpoint.web.Repositorios {
    public class ComentarioRepositorioCSV : IComentario //É preciso implementar aqui os métodos da Interface
    {
        public ComentarioModel Enviar_comentario (ComentarioModel comentario) {
            
            comentario.Status = "pendente"; //O comentário, ao ser enviado, fica pendente.

            if (File.Exists ("comentarios.csv")) {
                //Aplicando o Id do comentario
                comentario.IDcomentario = System.IO.File.ReadAllLines ("comentarios.csv").Length + 1;
            } else
                comentario.IDcomentario = 1;

            using (StreamWriter sw = new StreamWriter ("comentarios.csv", true)) {
                sw.WriteLine ($"{comentario.IDcomentario};{comentario.IDusuario};{comentario.Texto};{comentario.DataPostagem};{comentario.Status}");
            }

            return comentario;
        }

        public List<ComentarioModel> Listar_comentario () => CarregarDoCSV ();
        //Ele lista os comentários ao chamar o método "CarregarDoCSV". Não criar este método ou deixar o método abaixo público teria o mesmo efeito.

        /// <summary>
        /// Carrega a lista de comentários com os dados do CSV
        /// </summary>
        private List<ComentarioModel> CarregarDoCSV () 
        {
            List<ComentarioModel> lsComentarios = new List<ComentarioModel> ();

            //Abre o stream de leitura do arquivo
            string[] linhas = File.ReadAllLines ("comentarios.csv");

            //Lê cada registro no CSV
            foreach (string linha in linhas) 
            {
                //Verificando se é uma linha vazia. Haverá linhas vazias caso comentários sejam excluídos
                if (string.IsNullOrEmpty (linha))
                    continue; //Pula para o próximo registro do laço

                //Separa os dados da linha
                string[] dadosDaLinha = linha.Split (';');
                // O sinal '|' foi colocado porque alguém pode digitar ';' nos comentários.  

                //Cria o objeto com os dados da linha do CSV
                ComentarioModel comentario = new ComentarioModel (
                    idcomentario: int.Parse (dadosDaLinha[0]),
                    idusuario: int.Parse (dadosDaLinha[1]),
                    texto: dadosDaLinha[2],
                    dataPostagem: DateTime.Parse (dadosDaLinha[3]),
                    status: dadosDaLinha[4]
                );

                //Adicionando o comentário na lista
                lsComentarios.Add (comentario);

            }
            return lsComentarios;
        }

        public void Aprovar_comentario (int idcomentario) 
        {
            //Abre o stream da leitura do arquivo
            string[] linhas = File.ReadAllLines ("comentarios.csv");

            //Lê cada registro no CSV. Não dá para usar o foreach porque ele não funciona quando a variável é mudada no meio do comando.
            for (int i = 0; i < linhas.Length; i++) 
            {
                //Separa os dados da linha
                string[] dadosDaLinha = linhas[i].Split (';');
                // O sinal '|' foi colocado porque alguém pode digitar ';' nos comentários.

                ComentarioModel comentario = BuscarPorIdComentario(idcomentario);//Chama o método "BuscarPorIdComentario

                if (idcomentario.ToString () == dadosDaLinha[0]) 
                {   
                    linhas[i] = $"{comentario.IDcomentario};{comentario.IDusuario};{comentario.Texto};{comentario.DataPostagem};aprovado";
                    break;
                }
                
                // if (idcomentario.ToString () == dadosDaLinha[0]) 
                // {
                //     linhas[4] = "aprovado";
                //     break;
                // } //Se fizer assim, você substitui a linha inteira por "aprovado"
            }

            File.WriteAllLines ("comentarios.csv", linhas);

            
        } //É preciso preencher esse método.

        public void Reprovar_Excluir_comentario (int idcomentario) 
        {
            //Abre o stream da leitura do arquivo
            string[] linhas = File.ReadAllLines ("comentarios.csv");

            //Lê cada registro no CSV. Não dá para usar o foreach porque ele não funciona quando a variável é mudada no meio do comando.
            for (int i = 0; i < linhas.Length; i++) 
            {
                //Separa os dados da linha
                string[] dadosDaLinha = linhas[i].Split (';');
                // O sinal '|' foi colocado porque alguém pode digitar ';' nos comentários. 

                if (idcomentario.ToString () == dadosDaLinha[0]) 
                {
                    linhas[i] = "";
                    break;
                }
            }

            File.WriteAllLines ("comentarios.csv", linhas);

        }

        public ComentarioModel BuscarPorIdComentario (int idcomentario) {
            string[] linhas = System.IO.File.ReadAllLines ("comentarios.csv");

            for (int i = 0; i < linhas.Length; i++) {
                if (string.IsNullOrEmpty (linhas[i]))
                    continue;

                string[] dados = linhas[i].Split (';');

                if (dados[0] == idcomentario.ToString ()) {
                    ComentarioModel comentario = new ComentarioModel (
                        idcomentario: int.Parse (dados[0]),
                        idusuario: int.Parse (dados[1]),
                        texto: dados[2],
                        dataPostagem: DateTime.Parse (dados[3]),
                        status: dados[4]);
                    //Por alguma razão, não dá para usar a frase "dadosDaLinha"

                    return comentario;
                }
            }

            return null;
        }
    }
}