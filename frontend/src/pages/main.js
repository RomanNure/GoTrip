import React, {Component} from 'react'


export default class Main extends Component {
  constructor(props) {
    super(props);

    this.state = {

    }
  }

  render() {
    return (
      <>
      <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <a class="navbar-brand" href="#">Go&Trip</a>
      <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
        <ul class="navbar-nav mr-auto">
          <li class="nav-item active">
            <a class="nav-link" href="#">Home <span class="sr-only">(current)</span></a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Link</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Disabled</a>
          </li>
        </ul>
        <a class="nav-item nav-link my-2 my-lg-0 pl-md-0" href="#" data-toggle="modal" data-target="#registerModal">Create Account</a>
        <a class="nav-item nav-link my-2 my-lg-0 pl-md-0" href="#" data-toggle="modal" data-target="#loginModal">Login</a>
      </div>
    </nav>


    <div class="modal fade" id="registerModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Create Account</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <form action="">
              <div class="form-group">
                <label for="inputEmailReg">Email address</label>
                <input type="email" class="form-control" id="inputEmailReg" aria-describedby="emailHelp" placeholder="Enter email"/>
              </div>
              <div class="form-group">
                <label for="inputLoginReg">Login</label>
                <input type="text" class="form-control" id="inputLoginReg" aria-describedby="emailHelp" placeholder="Enter login"/>
              </div>
              <div class="form-group">
                <label for="inputPasswordReg">Password</label>
                <input type="password" class="form-control" id="inputPasswordReg" placeholder="Password"/>
              </div>
              <div class="form-group">
                <label for="inputPasswordReEnterReg">Re-enter password</label>
                <input type="password" class="form-control" id="inputPasswordReEnterReg" placeholder="Password"/>
              </div>
              <div class="form-check">
                <input type="checkbox" class="form-check-input" id="checkReg"/>
                <label class="form-check-label" for="checkReg">Check me out</label>
              </div>
              <button type="submit" class="btn btn-success mt-3 mb-3">Register</button>
            </form>
          </div>
        </div>
      </div>
    </div>

    <div class="modal fade" id="loginModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Login</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <form action="">
              <div class="form-group ">
                <label for="inputEmailLog">Email address</label>
                <input type="email" class="form-control" id="inputEmailLog" aria-describedby="emailHelp" placeholder="Enter email"/>
              </div>
              <div class="form-group">
                <label for="inputPasswordLog">Password</label>
                <input type="password" class="form-control" id="inputPasswordLog" placeholder="Password"/>
              </div>
              <div class="form-check">
                <input type="checkbox" class="form-check-input" id="checkLog"/>
                <label class="form-check-label" for="checkLog">Check me out</label>
              </div>
              <button type="submit" class="btn btn-success mt-3 mb-3">Login</button>
            </form>
          </div>
        </div>
      </div>
    </div>
    </>
    )
  }
}
