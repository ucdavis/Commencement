<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Commencement.Core.Domain.MajorCode>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Major List</title>
    
    <style type="text/css">
        ul li { float: left;display: inline-block;width: 275px; }
    </style>


</head>
<body>
    
    <ul>
        <% foreach (var major in Model) { %>
            <li><%: major.MajorName %></li>
        <% } %>
    </ul>

</body>
</html>
