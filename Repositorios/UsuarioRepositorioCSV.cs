using System;
using System.Collections.Generic;
using System.IO;
using carfel_checkpoint.web.Interfaces;
using carfel_checkpoint.web.Models;

namespace carfel_checkpoint.web.Repositorios
{
    public class UsuarioRepositorioCSV : IUsuario //É preciso implementar aqui os métodos da Interface
    {
        public UsuarioModel BuscarPorEmailESenha(string email, string senha)
        {
            List<UsuarioModel> usuariosCadastrados = CarregarDoCSV();

            //Percorro cada usuário da lista do CSV...
            foreach (UsuarioModel usuario in usuariosCadastrados)
            {
                if (usuario.Email == email && usuario.Senha == senha)
                {
                    return usuario;
                }
            }

            //Caso  sistema não encontre nenhuma combinação de email e senha retorna null
            return null;
        }

        public List<UsuarioModel> Listar() => CarregarDoCSV();
        //Ele lista os usuário ao chamar o método "CarregarDoCSV". Não criar este método e deixar o método abaixo público teria o mesmo efeito.

        /// <summary>
        /// Carrega a lista de usuários com os dados do CSV
        /// </summary>

         private List<UsuarioModel> CarregarDoCSV()
        {
            List<UsuarioModel> lsUsuarios = new List<UsuarioModel>();

            //Abre o stream de leitura do arquivo
            string[] linhas = File.ReadAllLines("usuarios.csv");

            //Lê cada registro no CSV
            foreach (string linha in linhas)
            {
                //Verificando se é uma linha vazia. Haverá linhas vazias no csv se usuários forem excluídos.
                if (string.IsNullOrEmpty(linha))
                    continue; //Pula para o próximo registro do laço

                //Separa os dados da linha
                string[] dadosDaLinha = linha.Split(';');

                //Cria o objeto com os dados da linha do CSV
                UsuarioModel usuario = new UsuarioModel
                (
                    id: int.Parse(dadosDaLinha[0]),
                    nome: dadosDaLinha[1],
                    email: dadosDaLinha[2],
                    senha: dadosDaLinha[3],
                    administrador: bool.Parse(dadosDaLinha[4])
                );

                //Adicionando o usuário na lista
                lsUsuarios.Add(usuario);
            }       
            return lsUsuarios;     
        }

        /// <summary>
        /// Excluir um registro do CSV
        /// </summary>
        /// <param name="id">O ID do usuário cadastrado</param>
        public void Excluir(int id)
        {
            //Abre o stream de leitura do arquivo
            string[] linhas = File.ReadAllLines("usuarios.csv");

            //Lê cada registro no CSV. Não dá para usar o foreach porque ele não funciona quando a variável é mudada no meio do comando.
            for (int i = 0; i<linhas.Length; i++)
            {
                //Separa os dados da linha
                string[] dadosDaLinha = linhas[i].Split(';');

                if (id.ToString() == dadosDaLinha[0])
                {
                    linhas[i] = "";
                    break;
                }
            }     

            File.WriteAllLines("usuarios.csv",linhas);

        }

        public UsuarioModel Cadastrar(UsuarioModel usuario)
        {
             if(File.Exists("usuarios.csv"))
             {
                 //Aplicando o ID
                 usuario.ID = System.IO.File.ReadAllLines("usuarios.csv").Length + 1;
                //O System.IO serve para diferenciar do método File.
             }
             else
                usuario.ID = 1;
             
            using(StreamWriter sw = new StreamWriter("usuarios.csv",true)){
                sw.WriteLine($"{usuario.ID};{usuario.Nome};{usuario.Email};{usuario.Senha};{usuario.Administrador}");
            }

            return usuario;
        }

        public UsuarioModel BuscarPorId(int id)
        {
            string[] linhas = System.IO.File.ReadAllLines("usuarios.csv");

            for (int i=0; i < linhas.Length; i++)
            {
                if(string.IsNullOrEmpty(linhas[i]))
                    continue;

                string[] dados = linhas[i].Split(';');

                if(dados[0] == id.ToString())
                {
                    UsuarioModel usuario = new UsuarioModel(                
                    id: int.Parse(dados[0]),
                    nome: dados[1],
                    email: dados[2],
                    senha: dados[3],
                    administrador: bool.Parse(dados[4]));
                
                    return usuario;
                }
            }

            return null;
        }


    }
       
}