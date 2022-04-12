window.onload = function () {

    let add = document.getElementsByClassName("btn-outline-primary");


    for (let i = 0; i < add.length; i++) {
        add[i].addEventListener('click', Addclick);
    }


    function Addclick(event) {
        AddToCart(event.target.value);
    }

}



function AddToCart(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Search/AddToCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {

                let data = JSON.parse(this.responseText);


                if (data.status == "success") {
                    $('body').append(
                        '<div></div><div id="myModal" class="modal fade" role="dialog">' +
                        '  <div class="modal-dialog">' +
                        '    <div class="modal-content">' +
                        '      <div class="modal-header">' +
                        '        <h3 class="modal-title">Success!</h3>' +
                        '        <button type = "button" class= "close" data-dismiss="modal">&times;</button>' +
                        '      </div>' +
                        '      <div class="modal-body">' +
                        '           <p>' + data.name + ' Added to cart' +
                        '</p>' +
                        '      </div>' +
                        '      <div class="modal-footer">' +
                        '        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>' +
                        '      </div>' +
                        '    </div>' +
                        '  </div>' +
                        '</div>');
                    $('#myModal').modal('show');
                    
                    //alert(data.name + " Added to cart");
                }
            }
        }
    }

    let req = { "Id": id }

    xhr.send(JSON.stringify(req));
}