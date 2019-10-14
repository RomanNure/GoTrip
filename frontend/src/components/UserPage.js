import React, { Component } from 'react';

export default class UserPage extends Component {
  constructor(props) {
      super(props);
      this.state = {
      }
    }

      render() {
        return (
            <div class="container bootstrap snippet">

            <div class="row ng-scope">
                <div class="col-md-4">
                    <div class="panel panel-default">
                        <div class="panel-body text-center">
                            <div class="pv-lg mr-3 ml-3"><img class="center-block img-circle img-responsive img-thumbnail rounded-circle thumb96 mt-3" src="images/Avatar.png" alt="Contact"/></div>
                            <h3 class="m0 text-bold">Audrey Hunt</h3>
                            <div class="mv-lg">
                                <p>Hello, I'm a just a dummy contact in your contact list and this is my presentation text. Have fun!</p>
                            </div>
                            <div class="text-center"><a class="btn btn-primary custom-btn mb-4 waves-effect #3abd94" href="">Send message</a></div>
                        </div>
                    </div>
                </div>
                <div class="col-md-8 panel panel-default">
                    <div>
                        <div class="panel-body">
                            <div class="pull-right">
                            </div>
                            <div class="h4 text-center mr-md-5 mt-5 mt-md-3">Account Information</div>
                            <div class="row pv-lg">
                                <div class="col-lg-2"></div>
                                <div class="col-lg-8">
                                    <form class="form-horizontal ng-pristine ng-valid">
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="inputContact1">Name</label>
                                            <div class="col-md-10">
                                                <input class="form-control" id="inputContact1" type="text" placeholder="" value="Audrey Hunt"/>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="inputContact2">Email</label>
                                            <div class="col-md-10">
                                                <input class="form-control" id="inputContact2" type="email" value="mail@example.com"/>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="inputContact3">Phone</label>
                                            <div class="col-md-10">
                                                <input class="form-control" id="inputContact3" type="text" value="(123) 465 789"/>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="inputContact4">Mobile</label>
                                            <div class="col-md-10">
                                                <input class="form-control" id="inputContact4" type="text" value="(12) 123 987 465"/>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="inputContact5">Website</label>
                                            <div class="col-md-10">
                                                <input class="form-control" id="inputContact5" type="text" value="http://some.wesbite.com"/>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="inputContact6">Address</label>
                                            <div class="col-md-10">
                                                <textarea class="form-control" id="inputContact6" row="4">Some nice Street, 1234</textarea>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="inputContact7">Social</label>
                                            <div class="col-md-10">
                                                <input class="form-control" id="inputContact7" type="text" value="@Social"/>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label" for="inputContact8">Company</label>
                                            <div class="col-md-10">
                                                <input class="form-control" id="inputContact8" type="text" placeholder="No Company"/>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-offset-2 col-sm-10">
                                                <div class="checkbox">
                                                    <label>
                                                        <input type="checkbox"/> Favorite contact?
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-offset-2 col-sm-10">
                                                <button class="btn btn-primary custom-btn" type="submit">Update</button>
                                            </div>
                                        </div>
                                    </form>
                                    <div class="text-right mb-3"><a class="text-muted" href="#">Delete this contact?</a></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        )
      }
}


/* HTML for UserPage */
/*

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <link rel="stylesheet" href="style.css">
    <title>Document</title>
</head>
<body>









</body>
</html>

*/


/* CSS for UserPage */
/*

div {
    font-family: 'Noticia Text', serif;
}

body {
    margin-top:20px;
    background:#f5f7fa;
}

.panel.panel-default {
    border-top-width: 3px;
}

.panel {
    box-shadow: 0 3px 1px -2px rgba(0,0,0,.14),0 2px 2px 0 rgba(0,0,0,.098),0 1px 5px 0 rgba(0,0,0,.084);
    border: 0;
    border-radius: 4px;
    margin-bottom: 16px;
}

.thumb48 {
    width: 48px!important;
    height: 48px!important;
}

.custom-btn {
    background-color: #3abd94!important;
    border: #3abd94!important;
}

*/

/* Change the link to the image
   Now it's in the '../../public/images/Avatar.png'
*/
