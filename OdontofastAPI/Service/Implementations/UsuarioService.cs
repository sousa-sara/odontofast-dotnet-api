using OdontofastAPI.DTO;
using OdontofastAPI.Model;
using OdontofastAPI.Repository.Interfaces;
using OdontofastAPI.Service.Interfaces;

namespace OdontofastAPI.Service.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> Login(string nrCarteira, string senha)
        {
            var usuario = await _usuarioRepository.GetUsuarioByCarteira(nrCarteira);
            if (usuario == null || usuario.SenhaUsuario != senha)
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }
            return usuario;
        }

        public async Task<UsuarioDTO> GetUsuarioByIdAsync(long id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return null;

            return new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                NomeUsuario = usuario.NomeUsuario,
                EmailUsuario = usuario.EmailUsuario,
                NrCarteira = usuario.NrCarteira,
                TelefoneUsuario = usuario.TelefoneUsuario
            };
        }

        public async Task<UsuarioDTO> UpdateUsuarioAsync(long id, UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                IdUsuario = id, // Aqui, usamos o 'id' passado como parâmetro
                NomeUsuario = usuarioDTO.NomeUsuario,
                SenhaUsuario = usuarioDTO.SenhaUsuario,
                EmailUsuario = usuarioDTO.EmailUsuario,
                NrCarteira = usuarioDTO.NrCarteira,
                TelefoneUsuario = usuarioDTO.TelefoneUsuario,
            };

            var updatedUsuario = await _usuarioRepository.UpdateAsync(usuario);

            // Retorna o UsuarioDTO com os dados atualizados
            return new UsuarioDTO
            {
                IdUsuario = updatedUsuario.IdUsuario,
                NomeUsuario = updatedUsuario.NomeUsuario,
                SenhaUsuario = updatedUsuario.SenhaUsuario,
                EmailUsuario = updatedUsuario.EmailUsuario,
                NrCarteira = updatedUsuario.NrCarteira,
                TelefoneUsuario = updatedUsuario.TelefoneUsuario
            };
        }

        public async Task<bool> DeleteUsuarioAsync(long id)
        {
            return await _usuarioRepository.DeleteAsync(id);
        }
    }
}
