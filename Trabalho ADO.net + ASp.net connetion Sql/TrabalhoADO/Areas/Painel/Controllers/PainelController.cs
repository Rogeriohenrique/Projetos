using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using TrabalhoADO.Areas.Painel.Models;

namespace TrabalhoADO.Areas.Painel.Controllers
{
    public class PainelController : Controller
    {
        //
        // GET: /Painel/Painel/
        public ActionResult _Index()
        {
            #region Conexão com Banco de Dados
            //TODO: 1º Criar conexão com banco de dadsos
            const string strConn = @"data source=Rogerio; Integrated Security=SSPI; Initial Catalog= TrabalhoAreas";
            var myConn = new SqlConnection(strConn);
            myConn.Open();
            #endregion

            #region Executando o comando no Banco de Dados
            //TODO 2º Executar um comando ...
            const string strQuery = "SELECT E.EMPRESAID, E.NOME, E.TELEFONE, E.ENDERECO, C.NOMECATEGORIA FROM EMPRESA E, CATEGORIA C WHERE E.CATEGORIAID = C.CATEGORIAID";
            var cmd = new SqlCommand(strQuery, myConn);
            var retorno = cmd.ExecuteReader();
            #endregion

            #region Processando o retorno do Banco de Dados ou não
            //TODO 3º Processar o retorno do DB ...
            var listaEmpresas = new List<Empresa>();

            while (retorno.Read())
            {
                var tempEmpresa = new Empresa();
                tempEmpresa.EmpresaId = int.Parse(retorno["EmpresaId"].ToString());
                tempEmpresa.Nome = retorno["Nome"].ToString();
                tempEmpresa.Telefone = retorno["Telefone"].ToString();
                tempEmpresa.Endereco = retorno["Endereco"].ToString();
                tempEmpresa.NomeCategoria = retorno["NomeCategoria"].ToString();

                listaEmpresas.Add(tempEmpresa);
            }
            #endregion

            return View(listaEmpresas);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            if (usuario.User.Equals("teste") && usuario.Pass.Equals("123"))
            {
                return RedirectToAction("_Index");
            }
            return View();
        }

        public ActionResult NovoCadastro()
        {
            #region Conexão com Banco de Dados
            //TODO: 1º Criar conexão com banco de dadsos
            const string strConn = @"data source=Rogerio; Integrated Security=SSPI; Initial Catalog= TrabalhoAreas";
            var myConn = new SqlConnection(strConn);
            myConn.Open();
            #endregion

            #region Execurtar um comando no Banco de Dados
            //TODO 2º Execurtar um comando no Banco de Dados
            const string strQuery = "SELECT * FROM CATEGORIA";
            var cmd = new SqlCommand(strQuery, myConn);
            var retorno = cmd.ExecuteReader();
            #endregion

            #region Processando o retorno do Banco de Dados
            //TODO 3º Processando o retorno do Banco de Dados
            var listaCategorias = new List<Categoria>();

            while (retorno.Read())
            {
                var tempCat = new Categoria();
                tempCat.CategoriaId = int.Parse(retorno["CategoriaId"].ToString());
                tempCat.NomeCategoria = retorno["NomeCategoria"].ToString();

                listaCategorias.Add(tempCat);
            }

            ViewBag.Categorias = listaCategorias;
            #endregion

            return View(new Categoria());
        }

        [HttpPost]
        public ActionResult NovoCadastro(Empresa empresa)
        {
            #region Conexão com Banco de Dados
            //TODO: 1º Criar conexão com banco de dadsos
            const string strConn = @"data source=Rogerio; Integrated Security=SSPI; Initial Catalog= TrabalhoAreas";
            var myConn = new SqlConnection(strConn);
            myConn.Open();
            #endregion

            if (ModelState == null || !ModelState.IsValid) return View();
            #region Executando o comando no Banco de Dados
            //TODO 2º Executar o comando no banco...
            var strQueryInsert = string.Format("INSERT INTO EMPRESA(NOME, TELEFONE, ENDERECO, CATEGORIAID) VALUES('{0}', '{1}','{2}','{3}')", empresa.Nome, empresa.Telefone, empresa.Endereco, empresa.CategoriaId);
            var cmd = new SqlCommand(strQueryInsert, myConn);
            cmd.ExecuteNonQuery();
            #endregion

            return RedirectToAction("_Index");
        }

        public ActionResult Delete(int id)
        {
            // Criando conexão com o DB...
            const string strConn = @"data source=Rogerio; Integrated Security=SSPI; Initial Catalog= TrabalhoAreas";
            var myConn = new SqlConnection(strConn);
            myConn.Open();

            // Executar um comando ...
            var strQueryDelete = string.Format("DELETE FROM EMPRESA WHERE EMPRESAID = {0}", id);
            var cmd = new SqlCommand(strQueryDelete, myConn);
            cmd.ExecuteNonQuery();
            return RedirectToAction("_Index");
        }

        public ActionResult Update(int id)
        {
            #region RETORNO EMPRESA

            #region
            // Criando conexão com o DB...
            const string strConn = @"data source=Rogerio; Integrated Security=SSPI; Initial Catalog= TrabalhoAreas";
            var myConn = new SqlConnection(strConn);
            myConn.Open();
            #endregion

            #region
            // Executar um comando ...
            string strQuery = string.Format("SELECT * FROM EMPRESA WHERE EMPRESAID = '{0}'", id);
            var cmd = new SqlCommand(strQuery, myConn);
            var retorno = cmd.ExecuteReader();
            #endregion

            #region
            // Processar o retorno do DB ...
            var listaEmpresas = new List<Empresa>();

            while (retorno.Read())
            {
                var tempEmpresa = new Empresa
                {
                    EmpresaId = int.Parse(retorno["EmpresaId"].ToString()),
                    Nome = retorno["Nome"].ToString(),
                    Telefone = retorno["Telefone"].ToString(),
                    Endereco = retorno["Endereco"].ToString()
                };

                listaEmpresas.Add(tempEmpresa);
            }
            retorno.Close();
            #endregion

            #endregion

            #region RETORNO CATEGORIA

            #region
            const string strQueryCat = "SELECT * FROM CATEGORIA";
            var cmdCat = new SqlCommand(strQueryCat, myConn);
            var retornoCat = cmdCat.ExecuteReader();
            #endregion

            #region
            var listaCategorias = new List<Categoria>();

            while (retornoCat.Read())
            {
                var tempCat = new Categoria();
                tempCat.CategoriaId = int.Parse(retornoCat["CategoriaId"].ToString());
                tempCat.NomeCategoria = retornoCat["NomeCategoria"].ToString();

                listaCategorias.Add(tempCat);
            }

            ViewBag.listaCategorias = listaCategorias;
            #endregion

            #endregion

            return View(listaEmpresas.FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Update(Empresa empresa, int id)
        {
            #region SALVANDO A EMPRESA
            // Criando conexão com o DB...
            const string strConn = @"data source=Rogerio; Integrated Security=SSPI; Initial Catalog= TrabalhoAreas";
            var myConn = new SqlConnection(strConn);
            myConn.Open();

            if (ModelState.IsValid)
            {
                // Executar um comando ...
                var strQueryUpdate = string.Format("UPDATE EMPRESA SET NOME = '{0}', TELEFONE = '{1}', ENDERECO = '{2}' " +
                                                   " WHERE EMPRESAID = '{3}'", empresa.Nome, empresa.Telefone, empresa.Endereco, id);
                var cmd = new SqlCommand(strQueryUpdate, myConn);
                cmd.ExecuteNonQuery();

                return RedirectToAction("_Index");
            }

            #endregion

            #region EXECUTANDO COMANDO DA CATEGORIA

            #region Selecionando os valores
            const string strQueryCat = "SELECT * FROM CATEGORIA";
            var cmdCat = new SqlCommand(strQueryCat, myConn);
            var retornoCat = cmdCat.ExecuteReader();
            #endregion

            #region Retornando os valores

            var listaCategorias = new List<Categoria>();

            while (ModelState.IsValid)
            {
                var tempCat = new Categoria();
                tempCat.CategoriaId = int.Parse(retornoCat["CategoriaId"].ToString());
                tempCat.NomeCategoria = retornoCat["NomeCategoria"].ToString();

                listaCategorias.Add(tempCat);
            }

            ViewBag.listaCategorias = listaCategorias;

            #endregion

            #endregion

            return View(empresa);
        }


        public ActionResult Detalhes(int id)
        {
            // Criando conexão com o DB...
            const string strConn = @"data source=Rogerio; Integrated Security=SSPI; Initial Catalog= TrabalhoAreas";
            var myConn = new SqlConnection(strConn);
            myConn.Open();

            // Executar um comando ...
            string strQuery = string.Format("SELECT * FROM EMPRESA WHERE EMPRESAID = '{0}'", id);
            var cmd = new SqlCommand(strQuery, myConn);
            var retorno = cmd.ExecuteReader();

            // Processar o retorno do DB ...
            var listaEmpresas = new List<Empresa>();

            while (retorno.Read())
            {
                var tempEmpresa = new Empresa
                {
                    EmpresaId = int.Parse(retorno["EmpresaId"].ToString()),
                    Nome = retorno["Nome"].ToString(),
                    Telefone = retorno["Telefone"].ToString(),
                    Endereco = retorno["Endereco"].ToString()
                };

                listaEmpresas.Add(tempEmpresa);
            }
            return View(listaEmpresas.FirstOrDefault());
        }

    }
}