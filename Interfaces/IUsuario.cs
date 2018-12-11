using System.Collections.Generic;
using carfel_checkpoint.web.Models;

namespace carfel_checkpoint.web.Interfaces
{
    public interface IUsuario
    {
        UsuarioModel Cadastrar (UsuarioModel usuario);
        List<UsuarioModel> Listar();
        void Excluir(int id);
        UsuarioModel BuscarPorEmailESenha(string email, string senha);
        UsuarioModel BuscarPorId(int id);

    }
}


//Model Interface Repositório Controller (é preciso criar a View também)