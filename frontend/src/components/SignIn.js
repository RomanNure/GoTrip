/*import React, { Component } from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import Link from '@material-ui/core/Link';
import Grid from '@material-ui/core/Grid';
import Box from '@material-ui/core/Box';
import Modal from '@material-ui/core/Modal';

import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';

function Copyright() {
    return (
        <Typography variant="body2" color="textSecondary" align="center">
            {'Copyright © '}
            <Link color="inherit" href="https://material-ui.com/">
                Go&Trips
            </Link>
            {' '}
            {new Date().getFullYear()}
            {'.'}
        </Typography>
    );
}

const useStyles = {//makeStyles(theme => ({
    '@global': {
        body: {
            backgroundColor: "red"
            //    backgroundColor: theme.palette.common.white,
        },
    },
    paper: {
        //    marginTop: theme.spacing(8),
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    avatar: {
        justifyContent: "center"
        //    margin: theme.spacing(1),
        //    backgroundColor: theme.palette.secondary.main,
    },
    form: {
        width: '100%', // Fix IE 11 issue.
        //    marginTop: theme.spacing(1),
    },
    submit: {
        //    margin: theme.spacing(3, 0, 2),
    },
};

export default class SignIn extends Component {
    constructor(props) {
        super(props);
        this.state = {
            tab: true
        }
    }
    render() {

        const classes = useStyles;
        console.log(' - SignIn')
        return (
            <Modal
                aria-labelledby="modal-title"
                aria-describedby="modal-description"
                open={this.state.tab}
                onBackdropClick={() => this.props.history.push('/')}
            >
                <Container component="main" maxWidth="xs" style={{ backgroundColor: "white", borderRadius: 30 }}>
                    <CssBaseline />
                    <div className={classes.paper}>
                        <Avatar style={classes.avatar}>
                            <LockOutlinedIcon />
                        </Avatar>
                        <Typography component="h1" variant="h5">
                            Sign in
                        </Typography>
                        <form className={classes.form} noValidate>
                            <TextField
                                variant="outlined"
                                margin="normal"
                                required
                                fullWidth
                                id="email"
                                label="Email Address"
                                name="email"
                                autoComplete="email"
                                autoFocus
                            />
                            <TextField
                                variant="outlined"
                                margin="normal"
                                required
                                fullWidth
                                name="password"
                                label="Password"
                                type="password"
                                id="password"
                                autoComplete="current-password"
                            />
                            <FormControlLabel
                                control={<Checkbox value="remember" color="primary" />}
                                label="Remember me"
                            />
                            <Button
                                type="submit"
                                fullWidth
                                variant="contained"
                                color="primary"
                                className={classes.submit}
                            >
                                Sign In
                    </Button>
                            <Grid container>
                                <Grid item xs>
                                    <Link href="#" variant="body2">
                                        Forgot password?
                            </Link>
                                </Grid>
                                <Grid item>
                                    <Link href="#" variant="body2">
                                        {"Don't have an account? Sign Up"}
                                    </Link>
                                </Grid>
                            </Grid>
                        </form>
                    </div>
                    <Box mt={8}>
                        <Copyright />
                    </Box>
                </Container>
            </Modal>
        )
    }

}
*/

import React, { Component } from 'react';
import axios from 'axios';

const display = {
    display: 'block',
    top: 70,
    width: 450,
    height: 550,
    borderRadius: 50
};
const hide = {
    display: 'none'
};
export default class SignIn extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            toggle: true,
            checked: false
        }
    }
    _onSubmit = () => {
        let { login, pass } = this.refs;
        console.log('post', login.value, pass.value)
        axios.post("https://go-trip.herokuapp.com/authorize/", { login: login.value, password: pass.value })
            .then(data => console.log('data => ', data))
            .catch(e => console.log('error => ', e))
    }

    render() {
        const modal = [];
        modal.push(
            <div className="modal" style={this.state.toggle ? display : hide}>
                <div className="modal-content" style={{ paddingTop: 50, justifyContent: "center" }}>
                    <div className="row" style={{ justifyContent: "center" }}>
                        <i className="material-icons ">lock</i>
                        <h4>Sign In</h4>
                    </div>
                    <div className="row" style={{ margin: 5 }}>
                        <div class="input-field col s6">
                            <input ref='login' placeholder="Login" id="login" type="text" class="validate" />
                        </div>
                    </div>
                    <div className="row" style={{ margin: 5 }}>
                        <div class="input-field col s6">
                            <input ref='pass' placeholder="Password" id="password" type="password" class="validate" />
                        </div>
                    </div>
                    <div className="row" style={{ marginLeft: 7 }}>
                        <label>
                            <input type="checkbox" class="filled-in" checked={this.state.checked} onClick={() => this.setState({ checked: !this.state.checked })} />
                            <span>Rememder me</span>
                        </label>
                    </div>
                    <div className="row" style={{ justifyContent: "center" }}>
                        <a className="btn waves-effect waves-light #e1f5fe light-blue lighten-5"
                            onClick={this._onSubmit} style={{ width: "90%", alignContent: "center" }}>Sign In</a>
                    </div>
                    <div className="row">
                        <a className="col s6" href="/registration"> Forgot password</a>
                        <a href="/registration"> Don't have an account? Sign Up</a>
                    </div>
                </div>
                <div className="modal-footer" style={{ justifyContent: "center" }}>
                    <div>Copyright @ Go&Trip 2019.</div>
                </div>
            </div>
        );
        return (
            <div class="container">
                <div>
                    {modal}
                </div>
            </div>

        );
    }
}