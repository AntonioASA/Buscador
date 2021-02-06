<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Buscador._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
          <asp:Panel runat="server" ID="pnlDatoaAlumno">
            <div>
                   <asp:Label Text="Nombre alumno" runat="server" />
                 <asp:TextBox id="txtNombreBus" runat="server"/> 
                <asp:Button id="btnBuscar" runat="server"  Text="Buscar" OnClick="btnBuscar_Click"/>
            </div>
            

         
            <asp:GridView ID="gvdAlumnos" runat="server" AutoGenerateColumns="false" DataKeyNames="ClaveAlumno">
                <Columns>
                    <asp:BoundField DataField="ClaveAlumno" HeaderText="Clave Alumno" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="ApellidoPaterno" HeaderText="Apellido Paterno" />
                    <asp:BoundField DataField="ApellidoMaterno" HeaderText="Apellido Materno" />
                    <asp:BoundField DataField="CorreoElectronico" HeaderText="Correo Electronico" />                 
                </Columns>
            </asp:GridView>
         </asp:Panel>
        <br />
        <asp:Button ID="btnDescarga" Text="Descarga" runat="server" OnClick="btnDescarga_Click" />
        <asp:Button ID ="btnExcel" Text="Descargar Excel" runat="server" OnClick="btnExcel_Click" />
        <asp:Button ID="btnEnvioCorreo" Text="Envio de correo" runat="server" OnClick="btnEnvioCorreo_Click" />
    </div>  
</asp:Content>
