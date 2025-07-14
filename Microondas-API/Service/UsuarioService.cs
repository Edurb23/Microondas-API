using Microondas_API.Models;
using System.Text.Json;

namespace Microondas_API.Service
{
    public static class UsuarioService
    {
        private const string ArquivoUsuarios = @"Data/usuarios.json";

        public static List<Usuario> CarregarUsuarios()
        {
            if (!File.Exists(ArquivoUsuarios))
                return new List<Usuario>();

            var json = File.ReadAllText(ArquivoUsuarios);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

        public static void SalvarUsuarios(List<Usuario> usuarios)
        {
            var pasta = Path.GetDirectoryName(ArquivoUsuarios);
            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            var json = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ArquivoUsuarios, json);
        }

        public static bool UsuarioExiste(string username)
        {
            return CarregarUsuarios().Any(u => u.Username == username);
        }

        public static bool CadastrarUsuario(Usuario usuario)
        {
            var usuarios = CarregarUsuarios();

            if (usuarios.Any(u => u.Username == usuario.Username))
                return false;

            usuarios.Add(usuario);
            SalvarUsuarios(usuarios);
            return true;
        }

        public static bool ValidarLogin(string username, string senha)
        {
            var usuarios = CarregarUsuarios();
            return usuarios.Any(u => u.Username == username && u.Senha == senha);
        }
    }
}
