
var bookDataFromLocalStorage = [];

$(".k-button k-primary btn-add-book").click(function(){
    $(".modal-content");
})

$(function(){
    loadBookData();
    var data = [
        {text:"資料庫",value:"image/database.jpg"},
        {text:"網際網路",value:"image/internet.jpg"},
        {text:"應用系統整合",value:"image/system.jpg"},
        {text:"家庭保健",value:"image/home.jpg"},
        {text:"語言",value:"image/language.jpg"}
    ]
    $("#book_category").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: data,
        index: 0,
        change: onChange

    });
    $("#bought_datepicker").kendoDatePicker();

    $("#book_grid").kendoGrid({
        dataSource: {
            data: bookDataFromLocalStorage,
            schema: {
                model: {
                    fields: {
                        BookId: {type:"int"},
                        BookName: { type: "string" },
                        BookCategory: { type: "string" },
                        BookAuthor: { type: "string" },
                        BookBoughtDate: { type: "string" },
                        //BookPublisher: { type: "string" }
                    }
                }
            },
            pageSize: 20,
        },
        toolbar: kendo.template("<class='book-grid-toolbar'><input class='book-grid-search' placeholder='我想要找......' type='text'></input></div>"),
        height: 550,
        sortable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
            { field: "BookId", title: "書籍編號",width:"10%"},
            { field: "BookName", title: "書籍名稱", width: "50%" },
            { field: "BookCategory", title: "書籍種類", width: "10%" },
            { field: "BookAuthor", title: "作者", width: "15%" },
            { field: "BookBoughtDate", title: "購買日期", width: "15%" },
            //{ field: "BookPublisher", title: "出版社", width: "10%" },
            { command: { text: "刪除", click: deleteBook }, title: " ", width: "120px" }
        ]
        
    });
    //查詢
    $(".book-grid-search").on("input propertychange",(function(){
        $("#book_grid").data("kendoGrid").dataSource.filter({
            filters: [
                { field: "BookName", operator: "contains", value: $(".book-grid-search").val()}
            ]
        });
    }));
})

function loadBookData(){
    bookDataFromLocalStorage = JSON.parse(localStorage.getItem("bookData"));
    if(bookDataFromLocalStorage == null){
        bookDataFromLocalStorage = bookData;
        localStorage.setItem("bookData",JSON.stringify(bookDataFromLocalStorage));
    }
}

//圖片轉換
function onChange(){
    $(".book-image").attr("src",$("#book_category").data("kendoDropDownList").value());
}



//刪除
function deleteBook(e){
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    for(i = 0;i<bookDataFromLocalStorage.length;i++){
        if(bookDataFromLocalStorage[i].BookId == dataItem.BookId){
            bookDataFromLocalStorage.splice(i,1);
            break;
        }
    }
    //更新資料
    localStorage.setItem("bookData",JSON.stringify(bookDataFromLocalStorage));
    $("#book_grid").data("kendoGrid").dataSource.data(bookDataFromLocalStorage);
}

//新增
$("#add_book").click(function(e){
    const book = {
        "BookId" : bookDataFromLocalStorage[bookDataFromLocalStorage.length - 1].BookId + 1,
        "BookName" : $("#book_name").val(),
        "BookCategory" : $("#book_category").data("kendoDropDownList").text(),
        "BookAuthor" : $("#book_author").val(),
        "BookBoughtDate" : kendo.toString($("#bought_datepicker").data("kendoDatePicker").value() , "yyyy-MM-dd"),
        //"BookPublisher" : $("#book_publisher").val()
    }
    
    bookDataFromLocalStorage.push(book);
    
    //更新資料
    localStorage.setItem("bookData",JSON.stringify(bookDataFromLocalStorage));
    $("#book_grid").data("kendoGrid").dataSource.data(bookDataFromLocalStorage);

    $("#window").data("kendoWindow").close();

    //欄位清空
    $("#book_name").value("");
    $("#book_author").value("");
    //$("#book_publisher").value("");
});

//彈出視窗
$(document).ready(function() {
    var myWindow = $("#window"),
        add = $("#add");

    add.click(function() {
        myWindow.kendoWindow({
            width: "500px",
            title: "新增書籍",
            visible: false,
            actions: [
                "Pin",
                "Minimize",
                "Maximize",
                "Close"
            ],
            close: onClose
        }).data("kendoWindow").center().open();
        add.fadeOut();
    });

    function onClose() {
        add.fadeIn();
    }
    
});
