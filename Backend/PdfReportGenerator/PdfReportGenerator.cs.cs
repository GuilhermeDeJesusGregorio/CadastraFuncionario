using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CadastraFuncionario.Backend
{
    public class PdfReportGenerator
    {
        private readonly string _connectionString;

        public PdfReportGenerator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GeneratePdfReport(string outputPath, string optionalImagePath = null)
        {
            // Query: pega todos os campos da tabela Funcionarios
            var dt = new DataTable();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Funcionarios", conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            // Calcula estatísticas básicas
            int total = dt.Rows.Count;
            decimal avgSalary = 0;
            DateTime? oldestAdmission = null;
            DateTime? newestAdmission = null;

            if (dt.Columns.Contains("Salario"))
            {
                decimal sum = 0;
                int countSal = 0;
                foreach (DataRow r in dt.Rows)
                {
                    if (r["Salario"] != DBNull.Value && decimal.TryParse(r["Salario"].ToString(), out var s))
                    {
                        sum += s;
                        countSal++;
                    }
                }
                if (countSal > 0) avgSalary = sum / countSal;
            }

            if (dt.Columns.Contains("DataAdmissao"))
            {
                foreach (DataRow r in dt.Rows)
                {
                    if (r["DataAdmissao"] != DBNull.Value && DateTime.TryParse(r["DataAdmissao"].ToString(), out var d))
                    {
                        if (!oldestAdmission.HasValue || d < oldestAdmission.Value) oldestAdmission = d;
                        if (!newestAdmission.HasValue || d > newestAdmission.Value) newestAdmission = d;
                    }
                }
            }

            // Agrupamento por cor/raça (se existir)
            Dictionary<string, int> byRace = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (dt.Columns.Contains("Cor") || dt.Columns.Contains("Raca") || dt.Columns.Contains("CorRaca"))
            {
                string col = dt.Columns.Contains("Cor") ? "Cor" : dt.Columns.Contains("Raca") ? "Raca" : "CorRaca";
                foreach (DataRow r in dt.Rows)
                {
                    var val = r[col] == DBNull.Value ? "Não informado" : r[col].ToString();
                    if (!byRace.ContainsKey(val)) byRace[val] = 0;
                    byRace[val]++;
                }
            }

            // Agrupamento por forma de pagamento (se existir)
            Dictionary<string, int> byPayment = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (dt.Columns.Contains("FormaPagamento") || dt.Columns.Contains("Forma_de_Pagamento"))
            {
                string col = dt.Columns.Contains("FormaPagamento") ? "FormaPagamento" : "Forma_de_Pagamento";
                foreach (DataRow r in dt.Rows)
                {
                    var val = r[col] == DBNull.Value ? "Não informado" : r[col].ToString();
                    if (!byPayment.ContainsKey(val)) byPayment[val] = 0;
                    byPayment[val]++;
                }
            }

            // Cria gráfico simples como bitmap (barra horizontal por raça)
            Bitmap chartBitmap = null;
            if (byRace.Count > 0)
            {
                chartBitmap = DrawBarChart(byRace, "Distribuição por Raça");
            }

            // --- Gerar PDF com iTextSharp ---
            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var doc = new Document(PageSize.A4, 36, 36, 54, 54);
                var writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Título
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);
                doc.Add(new Paragraph("Relatório de Funcionários - Arco Tecnologia", titleFont));
                doc.Add(new Paragraph($"Gerado em: {DateTime.Now:yyyy-MM-dd HH:mm}", normalFont));
                doc.Add(new Paragraph(" "));

                // Resumo
                var tblSummary = new PdfPTable(3) { WidthPercentage = 100 };
                tblSummary.SetWidths(new float[] { 1f, 1f, 1f });
                tblSummary.AddCell(new PdfPCell(new Phrase("Total de Registros", normalFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                tblSummary.AddCell(new PdfPCell(new Phrase("Salário Médio", normalFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                tblSummary.AddCell(new PdfPCell(new Phrase("Admissões (antiga/recente)", normalFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });

                tblSummary.AddCell(new PdfPCell(new Phrase(total.ToString(), normalFont)));
                tblSummary.AddCell(new PdfPCell(new Phrase(avgSalary > 0 ? avgSalary.ToString("C") : "N/D", normalFont)));
                tblSummary.AddCell(new PdfPCell(new Phrase(
                    (oldestAdmission.HasValue ? oldestAdmission.Value.ToString("yyyy-MM-dd") : "N/D") + " / " +
                    (newestAdmission.HasValue ? newestAdmission.Value.ToString("yyyy-MM-dd") : "N/D"),
                    normalFont)));

                doc.Add(tblSummary);
                doc.Add(new Paragraph(" "));

                // Tabela completa de funcionários (mostra colunas disponíveis)
                doc.Add(new Paragraph("Registros de Funcionários", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)));
                doc.Add(new Paragraph(" "));

                // Monta cabeçalho com todas as colunas que vieram da query
                int cols = dt.Columns.Count;
                var table = new PdfPTable(cols) { WidthPercentage = 100 };
                foreach (DataColumn c in dt.Columns)
                {
                    table.AddCell(new PdfPCell(new Phrase(c.ColumnName, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { BackgroundColor = BaseColor.GRAY });
                }

                foreach (DataRow r in dt.Rows)
                {
                    foreach (DataColumn c in dt.Columns)
                    {
                        var text = r[c] == DBNull.Value ? "" : r[c].ToString();
                        table.AddCell(new PdfPCell(new Phrase(text, normalFont)));
                    }
                }
                doc.Add(table);
                doc.Add(new Paragraph(" "));

                // Se existir distribuição por raça, adiciona tabela e chart
                if (byRace.Count > 0)
                {
                    doc.Add(new Paragraph("Distribuição por Raça", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                    var tblRace = new PdfPTable(2) { WidthPercentage = 50f };
                    tblRace.SetWidths(new float[] { 2f, 1f });
                    tblRace.AddCell(new PdfPCell(new Phrase("Raça/Cor", normalFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblRace.AddCell(new PdfPCell(new Phrase("Quantidade", normalFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    foreach (var kv in byRace)
                    {
                        tblRace.AddCell(new PdfPCell(new Phrase(kv.Key, normalFont)));
                        tblRace.AddCell(new PdfPCell(new Phrase(kv.Value.ToString(), normalFont)));
                    }
                    doc.Add(tblRace);
                    doc.Add(new Paragraph(" "));

                    // adiciona bitmap chart se gerado
                    if (chartBitmap != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            chartBitmap.Save(ms, ImageFormat.Png);
                            var itextImg = iTextSharp.text.Image.GetInstance(ms.ToArray());
                            itextImg.ScaleToFit(400f, 200f);
                            doc.Add(itextImg);
                        }
                        doc.Add(new Paragraph(" "));
                    }
                }

                // Se existir formas de pagamento
                if (byPayment.Count > 0)
                {
                    doc.Add(new Paragraph("Formas de Pagamento", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                    var tblPay = new PdfPTable(2) { WidthPercentage = 50f };
                    tblPay.SetWidths(new float[] { 2f, 1f });
                    tblPay.AddCell(new PdfPCell(new Phrase("Forma", normalFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblPay.AddCell(new PdfPCell(new Phrase("Quantidade", normalFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    foreach (var kv in byPayment)
                    {
                        tblPay.AddCell(new PdfPCell(new Phrase(kv.Key, normalFont)));
                        tblPay.AddCell(new PdfPCell(new Phrase(kv.Value.ToString(), normalFont)));
                    }
                    doc.Add(tblPay);
                    doc.Add(new Paragraph(" "));
                }

                // Inserir imagem opcional (dashboard)
                if (!string.IsNullOrEmpty(optionalImagePath) && File.Exists(optionalImagePath))
                {
                    try
                    {
                        var img = iTextSharp.text.Image.GetInstance(optionalImagePath);
                        img.ScaleToFit(450f, 300f);
                        doc.Add(new Paragraph("Visualização do Dashboard", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                        doc.Add(img);
                        doc.Add(new Paragraph(" "));
                    }
                    catch { /* não prejudica geração do PDF */ }
                }

                doc.Close();
                writer.Close();
            }

            return outputPath;
        }

        private Bitmap DrawBarChart(Dictionary<string, int> data, string title)
        {
            int width = 700;
            int height = 300;
            var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                int paddingLeft = 120;
                int paddingTop = 40;
                int barHeight = 20;
                int gap = 15;

                int maxVal = 1;
                foreach (var v in data.Values) if (v > maxVal) maxVal = v;

                int i = 0;
                foreach (var kv in data)
                {
                    int y = paddingTop + i * (barHeight + gap);
                    float pct = (float)kv.Value / maxVal;
                    int maxBarWidth = width - paddingLeft - 40;
                    int barWidth = (int)(pct * maxBarWidth);
                  
                    i++;
                }
            }
            return bmp;
        }
    }
}
