function projectsGrid() {
    var colNames = [
        'Id',
        'Gantt',
        'Text',
        'StartDate',
        'Duration',
        'Progress',
        'SortOrder'
    ];

    var columns = [];

    columns.push({
        name: 'Id',
        hidden: true,
        index:'Id',
        key: true
    });
    columns.push({
        name: 'Id',
        index: 'Id',
        formatter: function (cellvalue, options, rowObject) {
            return "<a href='/Gantt/Index/" + cellvalue + "' >Gantt</a>";
        },
        sortable: false
    });
    
    columns.push({
        name: 'Text',
        index: 'Text',
        width: 250,
        editable: true,
        editoptions: {
            size: 60,
            maxlength: 500
        },
        editrules: {
            required: true
        },
        //formatter: 'showlink',
        //formatoptions: {
        //    target: "_new",
        //    baseLinkUrl: '/Admin/GoToPost'
        //}
    });

    columns.push({
        name: 'StartDate',
        index: 'StartDate',
        width: 150,
        align: 'center',
        sorttype: 'date',
        datefmt: 'm/d/Y',
        editable: true,
        editoptions: {
            size: 60,
        },
    });

    columns.push({
        name: 'Duration',
        width: 200,
        sortable: false,
        editable: true,
        editoptions: {
            size: 3,
            maxlength: 3
        },
        //editrules: {
        //    required: true
        //}
    });

    columns.push({
        name: 'Progress',
        width: 200,
        sortable: false,
        //editable: true,
        //editoptions: {
        //    size: 3,
        //    maxlength: 3
        //},
        //editrules: {
        //    required: true
        //}
    });

    columns.push({
        name: 'SortOrder',
        width: 200,
        sortable: false,
        //editable: true,
        //editoptions: {
        //    size: 3,
        //    maxlength: 3
        //},
        //editrules: {
        //    required: true
        //}
    });

    $("#tableProject").jqGrid({
        url: '/Project/Projects',
        datatype: 'json',
        mtype: 'GET',
        height: 'auto',
        toppager: true,

        colNames: colNames,
        colModel: columns,

        pager: "#pagerProject",
        rownumbers: true,
        rownumWidth: 40,
        rowNum: 10,
        rowList: [10, 20, 30],

        sortname: 'StartDate',
        sortorder: 'desc',
        viewrecords: true,

        jsonReader: {
            repeatitems: false
        },
    });
    var addOptions = {
        url: '/Project/AddProject',
        addCaption: 'Add Project',
        processData: "Saving...",
        width: 900,
        closeAfterAdd: true,
        closeOnEscape: true
    };

    var deleteOptions = {
        url: '/Project/DeleteProject',
        caption: 'Delete Project',
        processData: "Saving...",
        msg: "Delete the Project?",
        closeOnEscape: true,
        afterSubmit: function (response, postdata) {

            var json = $.parseJSON(response.responseText);

            if (json) return [json.success, json.message, json.id];

            return [false, "Failed to get result from server.", null];
        }
    };

    $("#tableProject").navGrid("#pagerProject", { cloneToTop: true, edit: false, search: false }, {}, addOptions, deleteOptions);
};

function taskInit() {
    scheduler.config.xml_date = "%Y-%m-%d %H:%i";
    scheduler.init('scheduler_here', new Date(), "week");
    scheduler.load("/Event/Data", "json");

    var dp = new dataProcessor("/Event/Save");
    dp.init(scheduler);
    dp.setTransactionMode("POST", false);
};

$(document).ready(function () {
    $("#tabs").tabs({
        show: function (event, ui) {

            if (!ui.tab.isLoaded) {
                switch (ui.index) {
                    case 0:
                        projectsGrid();
                        break;
                    case 1:
                        taskInit();
                        break;
                };
                ui.tab.isLoaded = true;
            }
        }
    });
});