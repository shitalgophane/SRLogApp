<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SRLog.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .container {
  height: 400px;
  width: 100%;
  overflow: auto;
}
    </style>
    <script src="https://code.jquery.com/jquery-1.12.4.min.js" 
        integrity="sha384-nvAa0+6Qg9clwYCGGPpDQLVpLNn0fRaROjHqs13t4Ggj3Ez50XnGQqc/r8MhnRDZ" 
        crossorigin="anonymous">
</script>
    <script src="Scripts/jQuery.fixTableHeader.min.js"></script>
<%--<script src="jQuery.fixTableHeader.js"></script>--%>
    <script>
        $('#demo').fixTableHeader({
            fixHeader: true,
            fixFooter: false
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
   <div id="demo" class="container">
  <table>
    <thead>
      <tr>
      dsdsd
      </tr>
    </thead>
    <tbody>
 sdsd
    </tbody>
    <tfoot>
      <tr>
       sdsd
      </tr>
    </tfoot>
  </table>
</div>
    </form>
</body>
</html>
