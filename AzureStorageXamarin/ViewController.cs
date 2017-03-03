using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using UIKit;
using XamarinDiplomado.Participants.Startup;

namespace AzureStorageXamarin
{
	public partial class ViewController : UIViewController
	{
		string pais, localidad, archivo, latitud, longitud;

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			var startup = new Startup("Angel Torres", "jatorresn@outlook.com", 1, 1); 
			startup.Init();

			btnEnviar.TouchUpInside += async delegate
			{
				try
				{
					latitud = "4.698242";
					longitud = "-74.145755";
					pais = "Colombia";
					localidad = "Bogotá";
					archivo = "Foto1.jpg";

					CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=tallerxamarin;AccountKey=s+A8siTK0j504BTPkIBUT3e05t2OBoddrEXTkBMAbk1gEOH3ry7Vcs0ROAA0CPwfd9xL57Y1ywim+i+nDUNV5w==");
					CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
					CloudTable table = tableClient.GetTableReference("Ubicaciones");
					await table.CreateIfNotExistsAsync();

					UbicacionEntity ubica = new UbicacionEntity(archivo, pais);
					ubica.Latitud = latitud;
					ubica.Longitud = longitud;
					ubica.Localidad = localidad;

					TableOperation insertar = TableOperation.Insert(ubica);
					await table.ExecuteAsync(insertar);

					MessageBox("Guardado en Azure", "Table NoSQL");
				}
				catch (StorageException ex)
				{
					MessageBox("Error: ", ex.Message);
				}
			};

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public static void MessageBox(string Title, string message)
		{
			var Alerta = new UIAlertView();
			Alerta.Title = Title;
			Alerta.Message = message;
			Alerta.AddButton("OK");
			Alerta.Show();
		}
	}

	public class UbicacionEntity : TableEntity
	{
		public UbicacionEntity(string Archivo, string Pais)
		{
			this.PartitionKey = Archivo;
			this.RowKey = Pais;
		}
		public UbicacionEntity() { }
		public string Latitud { get; set; }
		public string Longitud { get; set; }
		public string Localidad { get; set; }
	}
}
