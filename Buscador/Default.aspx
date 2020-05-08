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
    </div>  
</asp:Content>
