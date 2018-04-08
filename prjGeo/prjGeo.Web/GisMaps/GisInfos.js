GisInfos = {}

GisInfos = function (mapname, intID,funcCall) {
    var self = this;
    var objData = null;

    $.ajax({
        type: 'GET',
        url: 'http://localhost:7769/api/GisApi/IndexGis',
        data: { id: intID, name: mapname },
        dataType: 'json',
        async: false,
        success: function (data, textStatus) {
            if (!data) {
                alert("data is null");
            }
            var markers, mploygon;
            if (data.markers) {
                markers = JSON.parse(data.markers);
            }
            if (data.mploygon) {
                mploygon = JSON.parse(data.mploygon);
            }
        //    alert("markers: " + markers.length + "mploygon: " + mploygon.length);
            objData = { 'markers': markers, 'mploygon': mploygon };
            if (funcCall) {
                funcCall(objData);
            }
            
           // alert(objData);
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            
        }
    });
 

    //如果有多个Polygan和marker的信息,该怎么处理
    this.mMarker =eval(objData.markers);
    this.mPloy = eval(objData.mploygon);
    this.mmarker1 = eval(objData.markers.marker1);
    this.mmarker2 = eval(objData.markers.marker2);
    ////alert(this.mMarker.marker1);
    ////alert(this.mPloy.ploy1);
    ////var mCircle = objData.markers;

    
    this.GetMarkers = function () {
        return this.mMarker;
    }
    this.GetPloy = function () {
        return this.mPloy;
    }
    this.DoGis=function(){
        base.initMap();
    }
};