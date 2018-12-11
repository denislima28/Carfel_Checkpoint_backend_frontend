using System.Collections.Generic;
using carfel_checkpoint.web.Models;

namespace carfel_checkpoint.web.Interfaces
{
    public interface IComentario
    {
        ComentarioModel Enviar_comentario (ComentarioModel comentario);
        List<ComentarioModel> Listar_comentario();
        void Aprovar_comentario (int idcomentario);
        void Reprovar_Excluir_comentario (int idcomentario);
        ComentarioModel BuscarPorIdComentario (int idcomentario);
    }
}