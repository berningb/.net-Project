//function myFunction() {
//    var x = document.getElementById("playlist");
//    var createPlaylist = document.createElement('form');
//    createPlaylist.setAttribute("action", "");
//    createPlaylist.setAttribute("method", "post");
//    x.appendChild(createPlaylist);

//    var nameLabel = document.createElement("label");
//    nameLabel.innerHTML = "Name of Playlist: ";
//    createPlaylist.appendChild(nameLabel);

//    var inputelement = document.createElement('input');
//    inputelement.setAttribute("type", "text");
//    inputelement.setAttribute("name", "dname");
//    createPlaylist.appendChild(inputelement);

//    var checkMarkButton = document.createElement('button');
//    checkMarkButton.innerHTML = "&#10003";
//    createPlaylist.appendChild(checkMarkButton);

//    var nameOfPlaylist = document.getElementsByName(inputelement).valueOf;
//    var myArray = [nameOfPlaylist];
//    myArray.push(nameOfPlaylist);
//    console.log()
//}



    //var nameOfPlaylist = document.getElementsByName(inputelement).value;
    //(function (nameOfPlaylist) {
    //    checkMarkButton.onClick = function () {
    //        var list = "<li>" + nameOfPlaylist + "</li>";
    //        document.getElementById('list').appendChild(list);
    //    };
//})(nameOfPlaylist);

var title = new Array();

function gatherTitle() {
    var titleValue = document.getElementById('title').value;
    title[title.length] = titleValue;

}

function listOfPlaylist() {
    var content = "";
    for (var i = 0; i < title.length; i++) {
        content += title[i] + "<br>";
    }
    document.getElementsByClassName('display')[0].innerHTML = content;
}


    



