using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using RDI.Utility;

namespace RDI.Utility
{
	/// <summary>
	/// Summary description for RDIDocuments.
	/// </summary>
	public class RDIDocuments
	{
		private SqlConnection con;

		private void OpenCon()
		{
			if (con.State != ConnectionState.Open)
			{
				con.Open();
			}
		}

		public RDIDocuments(string conString)
		{
			con = new SqlConnection(conString);
		}

		public RDIDocuments(SqlConnection ExistingCon)
		{
			con = ExistingCon;
		}

		public byte[] GetDocument(int pDocID)
		{
		    byte[] returnBytes = new byte[1];
			long retVal;

            string tempSql = "SELECT DOC FROM DOCS WHERE DOC_ID = " + pDocID;

			SqlCommand cmd = new SqlCommand(tempSql, con);

			OpenCon();
			SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

			while(rdr.Read())
			{
				int byteLen = (int)rdr.GetBytes(0, 0, null, 0, 0);
				returnBytes = new byte[byteLen];
				retVal = rdr.GetBytes(0, 0, returnBytes, 0, byteLen);
			}

			con.Close();

			return returnBytes;
		}

		public bool DocumentExists(int pDocID)
		{
		    string tempSql = "SELECT COUNT(*) FROM DOC_METADATA WHERE DOC_ID = " + pDocID;

			SqlCommand cmd = new SqlCommand(tempSql, con);
			cmd.CommandType = CommandType.Text;

			OpenCon();
			int result = Int32.Parse(cmd.ExecuteScalar().ToString());
			con.Close();

			return result >= 1;
		}

		public string GetDocumentMimeType(int pDocID)
		{
		    string tempSql = "SELECT DOC_MIME_TYPE FROM DOC_METADATA WHERE DOC_ID = " + pDocID;

			SqlCommand cmd = new SqlCommand(tempSql, con);

			OpenCon();
			object result = cmd.ExecuteScalar();
			return result.ToString();
		}

        public string GetDocumentFileName(int pDocID)
        {
            string tempSql = "SELECT FILE_NAME FROM DOC_METADATA WHERE DOC_ID = " + pDocID;

            SqlCommand cmd = new SqlCommand(tempSql, con);

            OpenCon();
            object result = cmd.ExecuteScalar();
            return result.ToString();
        }

		public string GetMimeType(string Extension)
		{
		    string tempSql = "SELECT MIME_TYPE FROM DOC_MIME_TYPES WHERE EXTENSION = '" + Extension + "'";

			SqlCommand cmd = new SqlCommand(tempSql, con);

			OpenCon();
			object result = cmd.ExecuteScalar();
			return result.ToString();
		}

		/// <summary>
		/// Returns the document mime type necessary to set response content type when displaying
		/// a document in browser
		/// </summary>
		/// <param name="pDocID"></param>
		/// <returns>string mime type</returns>
		public string GetDocumentType(int pDocID)
		{
		    string documentType;

			string tempSql = "SELECT b.DOC_TYPE FROM DOC_METADATA a, DOC_MIME_TYPES b ";
			tempSql += "WHERE a.DOC_EXTENSION = b.EXTENSION AND a.DOC_ID = " + pDocID;

			SqlCommand cmd = new SqlCommand(tempSql, con);

			OpenCon();
			try
			{
				documentType = cmd.ExecuteScalar().ToString();
			}
			catch
			{
				documentType = "Other";
			}

			return documentType;
		}

		/*public string GetDocumentMetadata(int pDocID)
		{
		    string retString = "";	//comma-delim list of metadata

		    string tempSql = "SELECT * FROM DOC_METADATA WHERE DOC_ID = " + pDocID;

			SqlCommand cmd = new SqlCommand(tempSql, con);
			OpenCon();
			SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

			while (rdr.Read())
			{
				retString = rdr["DOC_ID"].ToString();
                retString += ", " + rdr["TITLE"].ToString();
                retString += ", " + rdr["FILE_NAME"].ToString();
				retString += ", " + rdr["DOC_EXTENSION"].ToString();
				retString += ", " + rdr["DOC_UPD_DATE"].ToString();
				retString += ", " + rdr["DOC_MIME_TYPE"].ToString();
			}

            rdr.Close();
			return retString;
		}*/

        public DocumentMetaData GetDocumentMetadata(int pDocID)
        {
            DocumentMetaData dm = new DocumentMetaData();

            string tempSql = "SELECT * FROM DOC_METADATA WHERE DOC_ID = " + pDocID;

            SqlCommand cmd = new SqlCommand(tempSql, con);
            OpenCon();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (rdr.Read())
            {
                dm.DocId = rdr["DOC_ID"].ToString();
                dm.Title = rdr["TITLE"].ToString();
                dm.FileName = rdr["FILE_NAME"].ToString();
                dm.Extension = rdr["DOC_EXTENSION"].ToString();
                dm.UpdateDate = DateTime.Parse(rdr["DOC_UPD_DATE"].ToString());
                dm.MimeType = rdr["DOC_MIME_TYPE"].ToString();
            }

            rdr.Close();
            return dm;
        }

        public bool OpenDocumentInBrowser(int pDocID)
        {
            string sql = "SELECT DOC_EXTENSION FROM DOC_METADATA WHERE DOC_ID = " + pDocID;
            OpenCon();
            SqlCommand cmd = new SqlCommand(sql, con);
            string extension = cmd.ExecuteScalar().ToString();
            return OpenExtensionInBrowser(extension);
        }

        public bool OpenExtensionInBrowser(string pExtension)
        {
            switch (pExtension.ToLower())
            {
                //PDFs
                case ".pdf":
                //Images
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".bmp":
                case ".ico":
                case ".tif":
                case ".tiff":
                //Text
                case ".txt":
                    return true;
                default:
                    return false;
            }
        }

        public string SaveDocFileWithNewName(string TempFilePath, int DocID, string newName)
        {
            string strPath = SaveDocFile(TempFilePath, DocID);	//path to the file
            string newPath = TempFilePath + newName;

            try
            {
                if (File.Exists(newPath))
                {
                    //The file may already exist in the temp directory if this has been run twice.
                    File.Delete(newPath);
                }

                //Rename the file
                File.Move(strPath, TempFilePath + newName);
            }
            catch
            {
                newPath = string.Empty;
            }

            return newPath;
        }

	    public string SaveDocFile(string TempFilePath, int DocID)
		{
	        string strPath = "";	//path to the file

	        byte[] returnBytes = new byte[1];
			long retVal;

			string tempSql = "SELECT DOCS.DOC, DOC_METADATA.FILE_NAME FROM DOCS, DOC_METADATA WHERE " +
			                 "DOC_METADATA.DOC_ID = DOCS.DOC_ID AND DOCS.DOC_ID = " + DocID.ToString();

			SqlCommand cmd = new SqlCommand(tempSql, con);

			OpenCon();
			try
			{
				SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

				while (rdr.Read())
				{
					int byteLen = (int)rdr.GetBytes(0, 0, null, 0, 0);
					returnBytes = new byte[byteLen];
					retVal = rdr.GetBytes(0, 0, returnBytes, 0, byteLen);
					string strFileName = rdr["FILE_NAME"].ToString();
					strPath = TempFilePath + strFileName;

					// make sure the temp dir exists, create it.
					if (Directory.Exists(TempFilePath) == false)
					{
						Directory.CreateDirectory(TempFilePath);
					}

					// Create the file.
					using (FileStream fs = File.Create(strPath, 1024))
					{
						// Add some information to the file.
						fs.Write(returnBytes, 0, returnBytes.Length);
					}
				}
			}
			catch
			{
			}

			con.Close();

			return strPath;
		}
		public string DeleteDocFile(string TempFilePath, int DocID)
		{
			string tempSql;
			string strPath = "";	//path to the file
			string strFileName = "";
			SqlCommand cmd;

			byte[] returnBytes = new byte[1];

			tempSql = "SELECT DOC_METADATA.FILE_NAME FROM DOC_METADATA WHERE " +
				"DOC_METADATA.DOC_ID = " + DocID.ToString();

			cmd = new SqlCommand(tempSql, con);

			OpenCon();
			SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

			while(rdr.Read())
			{
				// get the file name and build the path
				strFileName = rdr["FILE_NAME"].ToString();
				strPath = TempFilePath + strFileName;

				// delete the file
				File.Delete(strPath);
			}

			con.Close();

			return strPath;
		}

		public void DeleteDocument(int DocID)
		{
			string tempSql;
			SqlCommand cmd;
			int result;

			OpenCon();

			tempSql = "DELETE FROM DOC_METADATA WHERE DOC_ID = " + DocID.ToString();
			cmd = new SqlCommand(tempSql, con);
			result = cmd.ExecuteNonQuery();
			
			tempSql = "DELETE FROM DOCS WHERE DOC_ID = " + DocID.ToString();
			cmd = new SqlCommand(tempSql, con);
			result = cmd.ExecuteNonQuery();

			con.Close();
		}

		public int AddDocument(byte[] Doc, string MimeType, string Title, string FileName, string Extension, string DocUpdDate, string AppName)
		{
			SqlCommand cmd = new SqlCommand("RDI_Doc_AddDocument", con);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter p1 = new SqlParameter("@Doc", SqlDbType.Image, Doc.Length);
			p1.Value = Doc;
			cmd.Parameters.Add(p1);

			// Brian Walch  9/16/2006
			// There is an irregular error occurring when some people upload files and the mime type is not saved correctly.
			// As known rules are discovered we will change the mime type via brute force until we can figure out the source of the error
			// The main example of the error is an MSWord doc that is uploaded and the mime type is saved is application/octet-stream
			string correctedMimeType = "";
			switch (Extension.ToLower().Trim())
			{
				case ".doc":
				case ".dot":
					correctedMimeType = "application/msword";
					break;
				case ".txt":
				case ".sql":
				case ".prc":
					correctedMimeType = "text/plain";
					break;
				case ".xls":
				case ".xlb":
				case ".xlt":
					correctedMimeType = "application/vnd.ms-excel";
					break;
				case ".msg":
					correctedMimeType = "application/msoutlook";
					break;
				case ".xlsx":
					correctedMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
					break;
				case ".jpg":
					correctedMimeType = "image/jpeg";
					break;
				case ".png":
					correctedMimeType = "image/png";
					break;
				case ".gif":
					correctedMimeType = "image/gif";
					break;
				case ".bmp":
					correctedMimeType = "image/bmp";
					break;
				case ".tif":
					correctedMimeType = "image/tiff";
					break;
				case ".tiff":
					correctedMimeType = "image/tiff";
					break;
				case ".htm":
					correctedMimeType = "text/html";
					break;
				case ".html":
					correctedMimeType = "text/html";
					break;
				case ".pdf":
					correctedMimeType = "application/pdf";
					break;
				case ".docx":
					correctedMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
					break;
				case ".exe":
					correctedMimeType = "application/octet-stream";
					break;
				case ".rtf":
					correctedMimeType = "application/rtf";
					break;
				case ".sxw":
				case ".sdw":
				case ".sdc":
					correctedMimeType = "application/vnd.sun.xml.writer";
					break;
				case ".sxc":
					correctedMimeType = "application/vnd.sun.xml.calc";
					break;
				case ".odt":
					correctedMimeType = "application/vnd.oasis.opendocument.text";
					break;
				case ".ods":
					correctedMimeType = "application/vnd.oasis.opendocument.spreadsheet";
					break;
				case ".xml":
					correctedMimeType = "application/xml";
					break;
				case ".xsd":
					correctedMimeType = "application/xsd";
					break;
				case ".zip":
					correctedMimeType = "application/zip";
					break;
				case ".mdi":
					correctedMimeType = "image/vnd.ms-modi";
					break;
				
				//add additional extensions here

				default:
					correctedMimeType = MimeType;
					break;
			}

			SqlParameter p2 = new SqlParameter("@MimeType", SqlDbType.VarChar, 50);
			p2.Value = correctedMimeType;
			cmd.Parameters.Add(p2);

			SqlParameter p3 = new SqlParameter("@Title", SqlDbType.VarChar, 250);
			p3.Value = Title.Replace(@"\", "");
			cmd.Parameters.Add(p3);

			SqlParameter p4 = new SqlParameter("@FileName", SqlDbType.VarChar, 250);
            p4.Value = FileName.Replace(@"\", ""); ;
			cmd.Parameters.Add(p4);

			SqlParameter p5 = new SqlParameter("@Extension", SqlDbType.VarChar, 10);
			p5.Value = Extension;
			cmd.Parameters.Add(p5);

			SqlParameter p6 = new SqlParameter("@DocUpdDate", SqlDbType.DateTime, 8);
			try
			{
				p6.Value = DateTime.Parse(DocUpdDate);
			}
			catch
			{
				p6.Value = System.DBNull.Value;
			}
			cmd.Parameters.Add(p6);

			SqlParameter p7 = new SqlParameter("@AppName", SqlDbType.VarChar, 50);
            p7.Value = AppName;  // FileName;
			cmd.Parameters.Add(p7);

			OpenCon();
            cmd.CommandTimeout = 240;
			object result = cmd.ExecuteScalar();
			con.Close();

			try
			{
				return Int32.Parse(result.ToString());
			}
			catch
			{
				return -1;
			}
		}

        public void UpdateDocument(byte[] doc, int id)
        {


            var p1 = new SqlParameter("@Doc", SqlDbType.Image, doc.Length) {Value = doc};

            var p2 = new SqlParameter("@Id", SqlDbType.BigInt) {Value = id};

            using (var cmd = new SqlCommand("RDI_Doc_UpdateDocument", con) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                OpenCon();
                cmd.CommandTimeout = 240;
                cmd.ExecuteNonQuery();
                con.Close();

            }
        }
	}

    public class DocumentMetaData
    {
        public string DocId { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public DateTime UpdateDate { get; set; }
        public string MimeType { get; set; }
    }
}
