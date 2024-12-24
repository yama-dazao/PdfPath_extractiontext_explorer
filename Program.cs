using System;
using System.IO;
using System.Windows.Forms;
using UglyToad.PdfPig;
class PdfPathExtractionTextToFile
{
    [STAThread]
    static void Main(string[] args)
    {
        // Windowsフォームのファイルダイアログを表示
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Title = "PDFファイルを選択してください",
            Filter = "PDFファイル (*.pdf)|*.pdf",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) // デスクトップを初期表示
        };

        // ユーザーがファイルを選択した場合のみ処理を進める
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string pdfPath = openFileDialog.FileName;

            // ダウンロードフォルダのパスを取得
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";

            // 保存ファイル名を作成
            string outputFilePath = Path.Combine(downloadsPath, "ExtractedText.txt");

            try
            {
                // PDFからテキストを抽出してメモ帳に保存
                using (var pdf = PdfDocument.Open(pdfPath))
                {
                    using (StreamWriter writer = new StreamWriter(outputFilePath))
                    {
                        writer.WriteLine("--- PDFから抽出した内容 ---\n");

                        foreach (var page in pdf.GetPages())
                        {
                            writer.WriteLine($"ページ {page.Number}:");
                            writer.WriteLine(page.Text);
                            writer.WriteLine("\n-------------------\n");
                        }
                    }
                }

                // 処理成功メッセージ
                Console.WriteLine("PDFの内容をテキストファイルに書き出しました。");
                Console.WriteLine($"保存先: {outputFilePath}");
                Console.WriteLine("メモ帳でファイルを開きます...");

                // メモ帳でファイルを開く
                System.Diagnostics.Process.Start("notepad.exe", outputFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("キャンセルされました。プログラムを終了します。");
        }
    }
}
