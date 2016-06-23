<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Tuple<Commencement.Core.Domain.SurveyField, Hashtable>>" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>

<div id="field-<%: Model.Item1.Id %>"></div>

<%--<script type="text/javascript" src="http://code.highcharts.com/highcharts.js"></script>--%>
<script type="text/javascript">
    $(function () {

        var responses = <%= new JavaScriptSerializer().Serialize(Model.Item2.Keys) %>;
        var counts = <%= new JavaScriptSerializer().Serialize(Model.Item2.Values) %>;

        var chart = new Highcharts.Chart({
            chart: {
                renderTo: '<%: "field-" + Model.Item1.Id %>', type: 'bar'
            },
            xAxis: {
                 categories: responses,
                 labels: { style: { width: '100px', margin: '10px' } }
            },
            plotOptions: {
                series: { 
                    stacking: 'normal',
                    dataLabels: {
                             enabled: true,  
                             color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'}                                                                         
                        }
            },
            series: [{data: counts}]
        });
    });
</script>