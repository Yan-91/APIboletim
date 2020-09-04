using APIBoletim.Context;
using APIBoletim.Domains;
using APIBoletim.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APIBoletim.Repositories
{
    public class AlunoRepository : IAluno
    {

        // Chamando a classe de conexâo do banco
        BoletimContext conexao = new BoletimContext();
        // Chamamos o objeto que poderá receber e executar os comandos do banco
        SqlCommand cmd = new SqlCommand();
        public Aluno Alterar(int id,Aluno a)
        {
            //Conectando
            cmd.Connection = conexao.Conectar();

            //parametros sendo colocados
            cmd.CommandText = "UPDATE Aluno SET Nome= @nome, RA = @ra, Idade= @idade WHERE IdAluno= @id";

            cmd.Parameters.AddWithValue("@nome", a.Nome);
            cmd.Parameters.AddWithValue("@ra", a.RA);
            cmd.Parameters.AddWithValue("@idade", a.Idade);

            cmd.Parameters.AddWithValue("@id", id);


            cmd.ExecuteNonQuery();
            //encerrar conexao
            conexao.Desconectar();

            return a;

        }

        public Aluno BuscarPorId(int id)
        {
            cmd.Connection = conexao.Conectar();
            // Vamos fazer um SELECT por ID
            cmd.CommandText = "SELECT *FROM Aluno WHERE IdAluno = @id ";
            // Mostrando para o codigo que Id la de cima é o @id
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader dados = cmd.ExecuteReader();

            Aluno a = new Aluno();
            while (dados.Read())
            {
                a.Idaluno = Convert.ToInt32(dados.GetValue(0));
                a.Nome = dados.GetValue(1).ToString();
                a.RA = dados.GetValue(2).ToString();
                a.Idade = Convert.ToInt32(dados.GetValue(3));

            }

            conexao.Desconectar();

            return a;
        }

        public Aluno Cadastrar(Aluno a)
        {
            cmd.Connection = conexao.Conectar();

            cmd.CommandText =
                "INSERT INTO Aluno (Nome, RA, Idade)" +
                "VALUES" +
                "(@nome, @ra, @idade)";
            cmd.Parameters.AddWithValue("@nome", a.Nome);
            cmd.Parameters.AddWithValue("@ra", a.RA);
            cmd.Parameters.AddWithValue("@idade", a.Idade);

            //Comando Responsavel por injetar os dados no banco
            //Post put Executenonquery - Get = executeReader
            cmd.ExecuteNonQuery();

            return a;
        }

        public void Excluir(int id)
        {
            //Conectando
            cmd.Connection = conexao.Conectar();

            //Mostrar que o @id é o Id
            cmd.CommandText = "DELETE FROM Aluno WHERE IdAluno= @id";
            cmd.Parameters.AddWithValue("@id", id);


            // Comando responsavel por injetar os dados no banco
            cmd.ExecuteNonQuery();

            //Desconectando
            conexao.Desconectar();

        }
    

        public List<Aluno> LerTodos()
        {
            // Abrindo a conexão
            cmd.Connection = conexao.Conectar();
            // Preparando a query (consulta)
            cmd.CommandText = "SELECT * FROM Aluno";

            SqlDataReader dados = cmd.ExecuteReader();

            // Criamos a lista pra guardar os alunos
            List<Aluno> alunos = new List<Aluno>();

            //Criando o laço
            while (dados.Read())
            {
                alunos.Add(
                    new Aluno()
                    {
                        Idaluno   = Convert.ToInt32(dados.GetValue(0)),
                        Nome      = dados.GetValue(1).ToString(),
                        RA        = dados.GetValue(2).ToString(),
                        Idade     = Convert.ToInt32(dados.GetValue(3))
                    }
                );


            }
            // Fechando conexão
            conexao.Desconectar();

            return alunos;
        }
    }
}
