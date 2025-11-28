using System;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text;

public class GerarRelatorio
{
    public class Funcionario
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Admissao { get; set; }
        public string SalarioBruto { get; set; }
    }

    public class Holerite
    {
        public string Titulo { get; set; }
        public string SalarioBruto { get; set; }
        public string DescontoINSS { get; set; }
        public string DescontoIRRF { get; set; }
        public string SalarioLiquido { get; set; }
    }

    public void GerarPDF(string caminho)
    {
        Document doc = new Document();



        // Cabeçalho
     
        doc.Add(new Paragraph("\n"));
        
        // -------------------------
        // 1. SALÁRIOS BRUTOS
        // -------------------------

        List<Funcionario> funcionarios = new List<Funcionario>()
        {
            new Funcionario { Id = "31", Nome = "Guilherme De Jesus Gregorio", Admissao = "2025-02-11", SalarioBruto = "R$ 3.500,00" },
            new Funcionario { Id = "32", Nome = "Lucas Felizardo", Admissao = "2020-02-04", SalarioBruto = "R$ 4.800,00" },
            new Funcionario { Id = "33", Nome = "Leonardo Dias", Admissao = "2023-10-10", SalarioBruto = "R$ 4.000,00" }
        };
;

        doc.Add(new Paragraph("\n"));

        // -------------------------
        // 2. HOLERITES
        // -------------------------
        doc.Add(new Paragraph("\n"));

        List<Holerite> holerites = new List<Holerite>()
        {
            new Holerite { Titulo = "2.1. Guilherme De Jesus Gregorio (ID 31)", SalarioBruto = "R$ 3.500,00", DescontoINSS = "405,65", DescontoIRRF = "85,83", SalarioLiquido = "R$ 3.008,52" },
            new Holerite { Titulo = "2.2. Lucas Felizardo (ID 32)", SalarioBruto = "R$ 4.800,00", DescontoINSS = "877,22", DescontoIRRF = "268,78", SalarioLiquido = "R$ 3.654,00" },
            new Holerite { Titulo = "2.3. Leonardo Dias (ID 33)", SalarioBruto = "R$ 4.000,00", DescontoINSS = "500,00", DescontoIRRF = "150,00", SalarioLiquido = "R$ 3.350,00" }
        };

        foreach (var h in holerites)
        {

            doc.Add(new Paragraph("\n"));
        }

        doc.Close();
    }

    
}
