using GAPPLE.Client.Services;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace GAPPLE.Client.Helpers
{
    internal class Functions
    {
        internal static int EAN8_CalcularDigitoVerificador(string data)
        {
            // Test string for correct length
            if (data.Length != 7 && data.Length != 8)
                return -1;

            if (!int.TryParse(data, out _))
                return -1;

            int sum = 0;
            for (int i = 6; i >= 0; i += -1)
            {
                int digit = int.Parse(data[i].ToString());
                if ((i & 1) == 1)
                    sum += digit;
                else
                    sum += digit * 3;
            }
            int mod = sum % 10;
            return mod == 0 ? 0 : 10 - mod;
        }

        internal static bool UPCA_Validar(string UPC)
        {
            int[] UPCData = new int[12];
            int i;
            // Check to see if inputted UPC Code is valid according to the Check Digit Verification Method
            int result;
            // Convert UPC Code into an Array to access specific digits easily
            for (i = 0; i < UPC.Length - 1; i++)
                UPCData[i] = Convert.ToInt32(UPC.Substring(i, 1));
            // Check validity of UPC Code
            result = ((UPCData[0] + UPCData[2] + UPCData[4] + UPCData[6] + UPCData[8] + UPCData[10]) * 3
                + (UPCData[1] + UPCData[3] + UPCData[5] + UPCData[7] + UPCData[9])) + UPCData[11];
            if (result % 10 == 0)
                // Valid UPC Code
                return true;
            else
                // Invalid UPC Code
                return false;
        }

        internal static bool UPC_Validar(string code)
        {
            if (code != new Regex("[^0-9]").Replace(code, ""))
            {
                // is not numeric
                return false;
            }

            // pad with zeros to lengthen to 14 digits
            switch (code.Length)
            {
                case 8:
                    code = "000000" + code;
                    break;
                case 12:
                    code = "00" + code;
                    break;
                case 13:
                    code = "0" + code;
                    break;
                case 14:
                    break;
                default:
                    // wrong number of digits
                    return false;
            }

            // calculate check digit
            int[] a =
            [
                int.Parse(code[0].ToString()) * 3,
                int.Parse(code[1].ToString()),
                int.Parse(code[2].ToString()) * 3,
                int.Parse(code[3].ToString()),
                int.Parse(code[4].ToString()) * 3,
                int.Parse(code[5].ToString()),
                int.Parse(code[6].ToString()) * 3,
                int.Parse(code[7].ToString()),
                int.Parse(code[8].ToString()) * 3,
                int.Parse(code[9].ToString()),
                int.Parse(code[10].ToString()) * 3,
                int.Parse(code[11].ToString()),
                int.Parse(code[12].ToString()) * 3,
            ];
            int sum = a[0] + a[1] + a[2] + a[3] + a[4] + a[5] + a[6] + a[7] + a[8] + a[9] + a[10] + a[11] + a[12];
            int check = (10 - (sum % 10)) % 10;
            // evaluate check digit
            int last = int.Parse(code[13].ToString());
            return check == last;
        }

        internal static string EAN13_CalcularDigitoVerificador(string z)
        {
            int ret = 0;
            int mul = 1;
            for (int i = 0; i <= 11; i++)
            {
                ret += int.Parse(z.Substring(i, 1)) * mul;
                if (mul == 1)
                    mul = 3;
                else
                    mul = 1;
            }
            ret = int.Parse(ret.ToString().Substring(ret.ToString().Length - 1, 1));
            string result;
            if (ret != 0)
                result = Math.Abs(ret - 10).ToString();
            else
                result = "0";
            return result;
        }

        internal static bool ValidarCUIT(string cuit)
        {
            // Limpiar el CUIT de cualquier formato que no sea numérico
            cuit = cuit.Replace("-", "").Trim();

            // Verificar que tenga 11 caracteres y que todos sean dígitos
            if (cuit.Length != 11 || !long.TryParse(cuit, out _))
                return false;

            // Pesos para cada dígito según el AFIP
            int[] pesos = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];

            // Suma acumulada
            int suma = 0;

            // Aplicar los pesos a los primeros 10 dígitos del CUIT
            for (int i = 0; i < 10; i++)
            {
                suma += int.Parse(cuit[i].ToString()) * pesos[i];
            }

            // Obtener el dígito verificador calculado
            int digitoVerificadorCalculado = 11 - (suma % 11);

            // Ajustar el dígito verificador en casos especiales
            if (digitoVerificadorCalculado == 11) digitoVerificadorCalculado = 0;
            if (digitoVerificadorCalculado == 10) digitoVerificadorCalculado = 9;

            // Comparar el dígito verificador calculado con el último dígito del CUIT
            int digitoVerificadorReal = int.Parse(cuit[10].ToString());

            return digitoVerificadorCalculado == digitoVerificadorReal;
        }

        internal static bool ValidarDNI(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                return false;

            dni = dni.Replace(".", "").Trim();

            if (!dni.All(char.IsDigit))
                return false;

            if (dni.Length < 7 || dni.Length > 8)
                return false;

            if (!int.TryParse(dni, out int dniNumero))
                return false;

            if (dniNumero < 1000000 || dniNumero > 99999999)
                return false;

            return true;
        }

        //internal static string InicialesDias(List<DiaEnum> dias)
        //{
        //    return string.Join(", ", Dias.ObtenerDias().Where(x => dias.Contains(x.Dia)).OrderBy(x => x.Dia).Select(x => x.Inicial));
        //}

        internal static bool ValidarComprobante(string tipoIVA, string letraComprobante)
        {
            if (tipoIVA == "RI")
            {
                return letraComprobante == "A" || letraComprobante == "B" || letraComprobante == "M";
            }
            else if (tipoIVA == "MT")
            {
                return letraComprobante == "C";
            }
            else if (tipoIVA == "EX")
            {
                return letraComprobante == "B" || letraComprobante == "C";
            }
            else if (tipoIVA == "CF")
            {
                return letraComprobante == "B";
            }

            return false;
        }

        internal static bool ValidarNumeroComprobante(string tipoComprobante, string numeroComprobante)
        {
            if (numeroComprobante.Length != 15)
            {
                return false;
            }
            else
            {
                string letra = Strings.Left(numeroComprobante, 1);

                if ((letra != "A" & letra != "B" & letra != "C" & letra != "M" & (tipoComprobante == "FAC" | tipoComprobante == "CRE" | tipoComprobante == "FCE" | tipoComprobante == "NCE" | tipoComprobante == "NDE"))
                    | (letra != "R" & letra != "X" & tipoComprobante == "RTO")
                    | (letra != "X" & (tipoComprobante == "REC" | tipoComprobante == "O/P"))
                    | (letra != "E" & (tipoComprobante == "FAE" | tipoComprobante == "RTE" | tipoComprobante == "DEE" | tipoComprobante == "CRX")))
                {
                    return false;
                }
                else
                {
                    if (!Information.IsNumeric(Strings.Mid(numeroComprobante, 3, 4)))
                    {
                        return false;
                    }

                    if (!Information.IsNumeric(Strings.Right(numeroComprobante, 8)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        internal static bool ValidarRUT(string rut)
        {
            rut = new string(rut.Where(char.IsDigit).ToArray());

            if (rut.Length != 12)
                return false;

            int[] pesos = { 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string numeroBase = rut.Substring(0, 11);
            int digitoReal = int.Parse(rut.Substring(11, 1));

            int suma = 0;
            for (int i = 0; i < 11; i++)
            {
                int digito = numeroBase[i] - '0';
                suma += digito * pesos[i];
            }

            int resto = suma % 11;
            int digitoCalculado = (11 - resto) % 11;

            // Ajustes según norma DGI
            if (digitoCalculado == 10) digitoCalculado = 1;
            if (digitoCalculado == 11) digitoCalculado = 0;

            return digitoCalculado == digitoReal;
        }

        public static bool ValidarCedulaUruguaya(string cedula)
        {
            cedula = cedula.Replace(".", "").Replace("-", "").Trim();

            if (cedula.Length != 8 || !cedula.All(char.IsDigit))
                return false;

            int[] coeficientes = { 2, 9, 8, 7, 6, 3, 4 };
            int suma = 0;

            for (int i = 0; i < 7; i++)
            {
                suma += (cedula[i] - '0') * coeficientes[i];
            }

            int digitoVerificadorCalculado = 10 - (suma % 10);
            if (digitoVerificadorCalculado == 10)
                digitoVerificadorCalculado = 0;

            int digitoVerificadorIngresado = cedula[7] - '0';

            return digitoVerificadorCalculado == digitoVerificadorIngresado;
        }

        public static List<T> LimitHierarchy<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> childSelector, Action<T, List<T>> setChildrenAction,
                                                int maxDepth, int currentDepth = 0)

        {
            if (currentDepth >= maxDepth || source == null)
                return [];

            return [.. source.Select(item =>
            {
                // Crear una copia del elemento actual
                var newItem = item;

                // Obtener hijos limitados
                var limitedChildren = LimitHierarchy(
                    childSelector(item),
                    childSelector,
                    setChildrenAction,
                    maxDepth,
                    currentDepth + 1
                );

                // Asignar los hijos limitados usando la acción proporcionada
                setChildrenAction(newItem, limitedChildren);

                return newItem;
            })];
        }

        public static string GetImageBase64(string urlImage)
        {
            var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fullPath = Path.Combine(wwwRootPath, urlImage.TrimStart('/'));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"No se encontró el archivo: {fullPath}");

            byte[] imageBytes = File.ReadAllBytes(fullPath);
            string base64String = Convert.ToBase64String(imageBytes);

            return "data:image/png;base64," + base64String;
        }

        //internal static TipoMovimientoStock SearchMovimiento(List<TipoMovimientoStock> movs, int idMovimientoBuscado)
        //{
        //    var mov = movs.Find(x => x.IdTipoMovimientoStock == idMovimientoBuscado);
        //    if (mov == null)
        //    {
        //        foreach (var m in movs)
        //        {
        //            if (m.Items != null)
        //            {
        //                mov = SearchMovimiento(m.Items, idMovimientoBuscado);
        //                if (mov != null)
        //                    return mov;
        //            }
        //        }
        //    }
        //    return mov;
        //}

        internal static async ValueTask<string> ObtenerObjeto(BackupService backupService, string nombrePagina, string idUsuario)
        {
            var data = await backupService.CargarObjeto(nombrePagina, idUsuario);
            if (string.IsNullOrEmpty(data))
                return null;

            ObjetoBackup objetoBackup = JsonConvert.DeserializeObject<ObjetoBackup>(data);
            if (objetoBackup.FechaCreacion > DateTime.Now.AddDays(-3))
                return JsonConvert.SerializeObject(objetoBackup.Objeto);
            else
            {
                await backupService.Delete(nombrePagina, idUsuario);
                return null;
            }
        }

        internal static async Task GuardarObjeto(BackupService localStorageService, string nombrePagina, object objeto, string idUsuario)
        {
            var data = new ObjetoBackup()
            {
                Objeto = objeto,
                FechaCreacion = DateTime.Now
            };
            await localStorageService.GuardarObjeto(nombrePagina, idUsuario, data);
        }

        internal static bool IdValido(int? id) => id.HasValue && id > 0;
    }
}
