


$(function () {
    var height = $(window).height();
    var width = $(window).width();
    var gridData = [];
    var PrjName = null;
    var PrjId = null;

    //$('#txtPrjName').combobox({
    //    url: "/mProject/GetListData",
    //    required: true,
    //    valueField: 'id',
    //    textField: 'CName'
    //});
    $("#btnFind").click(find);
    //  InitProjectCombo();
    InitialDatagrid();


    window.onresize = function () {
        height = $(window).height();
        width = $(window).width();
        $("div.middle").css("height", height);
        $("div.middle").css("width", width);
        if ($("body > div.middle > div.leftpanel").length > 0 && $("body > div.middle > div.leftpanel").position().left == 0) {
            $("div.middle>div.map").css("height", height - 100);
        } else {
            $("div.middle>div.map").css("height", height - 100);
        }
    };
    if ($("body > div.middle > div.leftpanel").length > 0 && $("body > div.middle > div.leftpanel").position().left == 0) {
        $("div.middle>div.map").css("height", height - 100);
    } else {
        $("div.middle>div.map").css("height", height - 100);
    }

    mapCls.initMap();

    function InitProjectCombo() {

        $.ajax({
            url: "/mProject/GetListData",
            type: 'post',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (!data) {
                    $.messager.alert('获取项目数据出错', data.errMsg);
                    return;
                }
                $('#txtProjId').combobox({
                    data: data,
                    valueField: 'id',
                    textField: 'Ccode',
                    onSelect: function (record) {
                        $("#txtPrjName").combobox('setValue', record.CName);
                        initProjCnt(record);
                    }

                });
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }, complete: function () {

            }
        });
    }
    function InitialDatagrid() {
          $('#dg').datagrid({
               loader: loadData,
               pagination: true,
               pageList: [5, 10, 50, 100, 500],
               pageSize: 5,
               rownumbers: false,
               fitColumns: true,
               singleSelect: true,
           //    selectOnCheck: true,
           //    checkOnSelect:true,
               cache: true,
               striped: true,
               autoRowHeight: true,
               nowrap: false,
               loadMsg: "正在加载数据，请稍等…",
               //onCheck: function (rowIndex, rowData) {
               //    gridCheck(rowIndex, rowData,1);
               //},
               //onUncheck:function(rowIndex, rowData){
               //    gridCheck(rowIndex, rowData, 0);
               //},
               //rowStyler: function (rowIndex, rowData) {
               //    return 'background-color:#fff;';
              //},
               onSelect: function (index, row) {
                 //  $('#dg1').datagrid('options').selectRow = index;
                   getLayer(row.CName);
               },
               onClickCell: function (rowIndex, field, value) {
                  // MouseCellClick(rowIndex, field, value);
               },
               onLoadSuccess: function (data) {
                     $(".datagrid-header-check").html(""); //去掉全选的Check，因其按钮操作不能进check事件。
                     $(".easyui-tooltip").tooltip(
                          {
                              onShow: function () {
                                  $(this).tooltip('tip').css({
                                      width: '150',
                                      height: '35',
                                      boxShadow: '1px 1px 3px #292929'
                                  });
                              },
                              position: 'bottom'
                          });

                     if (data) {
                         $.each(data.rows, function (index, item) {
                             if (item.IsVisible && item.IsVisible===1) {
                                 $('#dg').datagrid('checkRow', index);
                             }
                         });
                     }

              
               },
               columns: [[
                   //{ field: 'IsVisible', checkbox: true, align: "center" },
                   { field: 'CName', title: '项目名', width: 150, align: "center" },
                   { field: 'Ccode', title: '项目编号', width: 100, align: "center" },
               ]]
           });

    }

    function loadData(param, success, error) {
        if ((!PrjName || PrjName == "") && (!PrjId || PrjId == "")) {
            success({ total: 0, rows: [] });
            return;
        }
        var grid = $("#dg" ).datagrid("getPager" ).data("pagination" ).options;
            
        var obj = {
            PrjName: PrjName,
            PrjId: PrjId,
            pager: {
                order:"desc",
                sort:"id",
                rows: param.rows,
                page: param.page
            }
        }
        $.ajax({
            url: "/mProject/GetListByFilter",
            type: 'post',
            data: JSON.stringify(obj),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.errMsg) {
                    $.messager.alert('查询出错', data.errMsg);
                    success({ total: 0, rows: [] });
                    return;
                }

                success(data);
               // gridData = data;

              //  $("#dg").datagrid('reload');

            },
            error: function (jqXHR, textStatus, errorThrown) {

            }, complete: function () {

            }
        });
    }

    function find() {
        //  var PrjName = $("#txtPrjName").combobox('getText');
        PrjName = $("#txtPrjName").textbox('getText');
        PrjId = $("#txtPrjId").textbox('getText');
        if ((!PrjName || PrjName=="")&&(!PrjId || PrjId=="")) {
            $.messager.alert('系统提示', '请输入查询条件!');
            return;
        }
        $("#dg").datagrid('reload');
          
    }

    function getLayer(PrjName) {
        var param = {
            PrjName: PrjName
        }
        $.ajax({
            url: "/mMap/GetListByFilter",
            type: 'post',
            data: JSON.stringify(param),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.errMsg) {
                    $.messager.alert("取kml文件出错", data.errMsg);
                    return;
                }
                if (data.length < 1) {
                    $.messager.alert("提示：", "此项目没有kml文件图层！");
                    return;
                }

                mapCls.clearMap();
                mapCls.initMap();
                var urls = [];
                for (var i = 0; i < data.length; i++) {
                    urls.push({ name: data[i].LayerName, url: data[i].KmlPath });
                }
                mapCls.addKmls(urls);

                //var urls = [];
                //var lstShow = [];
                //for (var i = 0; i < data.length; i++) {
                //    urls.push([data[i].KmlPath]);
                //    if (data[i].IsVisible === 1) {
                //        lstShow.push(i);
                //    }
                //}
                //mapCls.addKmls(urls);
                //mapCls.showKmls(lstShow);

            },
            error: function (jqXHR, textStatus, errorThrown) {

            }, complete: function () {

            }
        });
    }

    function gridCheck(rowIndex, rowData, bCheck) {
        var row = rowData;
        row.IsVisible = bCheck;
        var param ={
            action: "modify",
            model: rowData
            };
        //更新图层状态。
        $.ajax({
            url: "/mKml/SaveData",
            type: 'post',
            data: JSON.stringify(param),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.errMsg && data.errMsg!="") {
                    $.messager.alert('更新图层显示状态出错', data.errMsg);
                    return;
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {

            },
            complete: function () {

            }
        });
    }



})
