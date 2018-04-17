objMaps = {}
var arrMarks;
var arrPloys = [];
var arrCirs = [];
var arrBounds = [];

objMaps.initData = function () {



    //解析数据,动态生成各个Marker,Ploygon和Circle,同时填充各个数组

}

objMaps.addMap = function (flon, flat, objData, mapname) {
    var emap_url = 'http://t4.tianditu.com/vec_c/wmts';
    var emapanno_url = 'http://t4.tianditu.com/cva_c/wmts';
    var img_url = 'http://t4.tianditu.com/img_c/wmts';
    var imganno_url = 'http://t4.tianditu.com/cia_c/wmts';

    //电子地图
    var emap_layer = new L.TileLayer.WMTS(emap_url,
                               {
                                   tileSize: 256,
                                   layer: 'vec',
                                   style: "default",
                                   tilematrixSet: "c",
                                   format: "tile"
                               }
                              );
    //电子地图注记
    var emapanno_layer = new L.TileLayer.WMTS(emapanno_url,
                               {
                                   tileSize: 256,
                                   layer: 'cva',
                                   style: "default",
                                   tilematrixSet: "c",
                                   format: "tile"
                               }
                              );
    //影像地图						  
    var img_layer = new L.TileLayer.WMTS(img_url,
                               {
                                   tileSize: 256,
                                   layer: 'img',
                                   style: "default",
                                   tilematrixSet: "c",
                                   format: "tile"

                               }
                              );
    //影像地图注记						  
    var imganno_layer = new L.TileLayer.WMTS(imganno_url,
                               {
                                   tileSize: 256,
                                   layer: 'cia',
                                   style: "default",
                                   tilematrixSet: "c",
                                   format: "tile"
                               }
                              );
    //定义坐标中心点
    ////var flon = 114.31; 经度
    ////var flat = 30.59;  纬度

    var map = L.map('map', {
        crs: L.CRS.EPSG4326,
        center: {
            lon: flon,
            lat: flat
        },
        zoom: 15
    });


    var dlgLayer = L.layerGroup().addLayer(emap_layer, emapanno_layer);
    var imgLayer = L.layerGroup().addLayer(imganno_layer, img_layer);
    //map.addLayer(dlgLayer);
    //map.addLayer(imgLayer);
    map.addLayer(img_layer);
    map.addLayer(imganno_layer);

   
    var mlatlng = [];// objData.mmarker1;
    mlatlng[0, 0] = flat;
    mlatlng[0, 1] = flon;
    
    var marker1 = L.marker(mlatlng).addTo(map);
    mlatlng = objData.mmarker2;
    ////mlatlng[0, 0] = 30.50
    ////mlatlng[0, 1] = 114.29;
    var marker2 = L.marker(mlatlng).addTo(map);
    //var marker1=L.marker([30.59,114.31]).addTo(map);
    //var marker2 = L.marker([30.50, 114.29]).addTo(map);
    var bounds = new L.Bounds([
		[30, 114],
		[30.2, 114.3]
    ]);
   
    //console.log(map.getSize());	

    var popup1 = L.popup()
		.setLatLng([30.50, 114.20])
   
		.setContent('<p><h3>Hello world!</h3><br />This is a nice popup.</p>')

    var popup2 = marker1.bindPopup(popup1).getPopup();
    marker2.bindPopup(popup2);
    marker2.setPopupContent(mapname);

    var polygon = L.polygon(eval(objData.mPloy.ploy1), { color: "#F00" }).addTo(map);

    ////var polygon = L.polygon([
	////	[30.59, 114.31],
	////	[30.59, 114.32],
	////	[30.57, 114.33]
    ////], { color: "#F00" }).addTo(map);
    polygon.bindPopup(popup1)

    var circlemarker1 = new L.CircleMarker([30.55, 114.25], { radius: 100, weight: 2 });
    circlemarker1.addTo(map).bindPopup(popup1)

    var circlemarker2 = new L.CircleMarker([33.62, 114.28], { radius: 100, weight: 2 });
    circlemarker2.addTo(map).bindPopup(popup1)
    var popup = L.popup();
    function onMapClick(e) {
        ////alert("You clicked the map at " + e.latlng);
        popup
        .setLatLng(e.latlng)
        .setContent("You clicked the map at " + e.latlng.toString())
        .openOn(map);
    }
    marker1.on('click', onMapClick);
    marker2.on("click", function () { alert("0") });
    map.on('click', onMapClick);

}



mapCls = {
    map: null,
    kmlLayers:[],
    initMap: function (mapId) {
        var emap_url = 'http://t4.tianditu.com/vec_c/wmts';
        var emapanno_url = 'http://t4.tianditu.com/cva_c/wmts';
        var img_url = 'http://t4.tianditu.com/img_c/wmts';
        var imganno_url = 'http://t4.tianditu.com/cia_c/wmts';

        //电子地图
        var emap_layer = new L.TileLayer.WMTS(emap_url,
                                   {
                                       tileSize: 256,
                                       layer: 'vec',
                                       style: "default",
                                       tilematrixSet: "c",
                                       format: "tile"
                                   }
                                  );
        //电子地图注记
        var emapanno_layer = new L.TileLayer.WMTS(emapanno_url,
                                   {
                                       tileSize: 256,
                                       layer: 'cva',
                                       style: "default",
                                       tilematrixSet: "c",
                                       format: "tile"
                                   }
                                  );
        //影像地图						  
        var img_layer = new L.TileLayer.WMTS(img_url,
                                   {
                                       tileSize: 256,
                                       layer: 'img',
                                       style: "default",
                                       tilematrixSet: "c",
                                       format: "tile"

                                   }
                                  );
        //影像地图注记						  
        var imganno_layer = new L.TileLayer.WMTS(imganno_url,
                                   {
                                       tileSize: 256,
                                       layer: 'cia',
                                       style: "default",
                                       tilematrixSet: "c",
                                       format: "tile"
                                   }
                                  );
        var ts = this;
        var mid = mapId ? mapId : 'map';
        ts.map = L.map(mid, {
            crs: L.CRS.EPSG4326,
            center: {
                lon: 114.31, //经度
                lat: 30.59 // 纬度
            },
            zoom: 15
        });
        ts.map.addLayer(img_layer);
        ts.map.addLayer(imganno_layer);
    },

    addKmls: function (lstKmlUrl) {
        var map = this.map;
        if (this.kmlLayers.length > 0) {
            for (var i = 0; i < kmlLayers.length;i++) {
                this.kmlLayer[i].remove();
            }
           
        }
        this.kmlLayer = [];
               
        //add kmls
        for (var i = 0; i < lstKmlUrl.length;i++) {
            var layer = new L.KML(lstKmlUrl[i], { async: true });
            layer.on("loaded", function (e) {
                map.fitBounds(e.target.getBounds());
            });
            map.addLayer(layer);
       //     map.addControl(new L.Control.Layers({}, { 'kml': layer }));
            this.kmlLayer.push(layer);
        }

        /*

        //      var kmlLayer = new L.KML("../kml/test.xml", { async: true });
        var kmlLayer = new L.KML("/KML/kml/xml/Cd.xml", { async: true });
        kmlLayer.on("loaded", function (e) {
            ts.map.fitBounds(e.target.getBounds());
        });
        ts.map.addLayer(kmlLayer);
        ts.map.addControl(new L.Control.Layers({}, { 'kml': kmlLayer }));
        //add end */
    },
    setView: function (lat,lon,zoom) {
        if (this.map) {
            this.map.setView(L.latLng(lat, lon), zoom);
        }
    },

    addMarker: function () {

    },
    addMarkers: function () {

    },
    //obj: {"popMsg":"poly1","popCoor":[31.3579,112.86321], "color":"#F00","coordinates":[[31.35914,112.86394],[31.3579,112.86321]]}
    addPolygon: function (obj) {
        if (!obj) return;
        var co = obj.color ? obj.color : "#f00";
        var polygon = L.polygon(obj.coordinates, { color: co}).addTo(this.map);
        if (obj.popMsg) {
            var coor = obj.popCoor ? obj.popCoor : obj.coordinates[0];
            var popup = L.popup()
               .setLatLng(coor)
               .setContent(obj.popMsg);       //'<p><h3>Hello world!</h3><br />This is a nice popup.</p>')
            polygon.bindPopup(popup);
        }
        polygon.on('mousemove', function (e) {
            polygon.setStyle({ color: "#ff7800", weight: 1 });
        });
        polygon.on('mouseout', function (e) {
            polygon.setStyle({ color: "#f00", weight: 1 });
        });
    },
    addPolygons: function (objs) {
        var vt = this;
        var len = objs? objs.length:0;
        for (var i = 0; i < len; i++) {
            vt.addPolygon(objs[i]);
        }
        
    }
    
}


