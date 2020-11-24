import React, {Component} from 'react'
import './registerStyle.css'

class Accounts extends Component {
    constructor(props) {
        super(props);
        this.state = {
            fullName: '',
            email: "",
            password: "",
            emailLog: "",
            passwordLog: "",
            logIn: false,
            data: [],
            error: ""
        };
        this.responseRef = new React.createRef();

    }
    handleName = (e) => {
        this.hideResponse()
        this.setState({fullName: e.target.value})
    }

    handleEmail =(e) => {
        this.hideResponse()
        this.setState({email: e.target.value});
    }

    handlePassword = (e) => {
        this.hideResponse()
        this.setState({password: e.target.value})
    }

    handleEmailLog =(e) => {
        this.hideResponse()
        this.setState({emailLog: e.target.value});
    }

    handlePasswordLog = (e) => {
        this.hideResponse()
        this.setState({passwordLog: e.target.value})

    }

    checkLogin = async () => {
        if(this.state.emailLog === "" || this.state.passwordLog === "")
            return;
        const options = {
            method: "POST",
            headers: {
                "Content-Type": "text/plain",
                // "Access-Control-Allow-Origin": "*",
                // "Accept": "*/*"
            },
            body: `grant_type=password&username=${this.state.emailLog}&password=${this.state.passwordLog}`
        }

        await fetch('http://localhost:8080/api/token', options)
            .then(res => {
                if(res.status === 400) {
                    throw new DOMException("Username or password wrong.")
                }
                return res.json();
            })
            .then(
                (result) => {
                    localStorage.setItem("TOKEN", result["access_token"])
                    this.setState({logIn: true, data: JSON.stringify(result)})
                }
            )
            .catch( (error) => {
                this.setState({error: error});
            })
        if(!this.checkErrors())
            this.props.history.push('/users')
    }

    createUser = async () => {
        if(this.state.email === "" || this.state.password === "" || this.state.fullName === "")
            return;

        const newUser = {
            "fullName": this.state.fullName.toString(),
            "email": this.state.email.toString(),
            "password": this.state.password.toString()
        }

        const options = {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": 'Bearer ' + localStorage.getItem("TOKEN")
            },
            body: JSON.stringify(newUser),
        }

        await fetch('http://localhost:8080/api/users', options)
            .then(res=> {
                if(res.status === 302)
                    throw new DOMException("Email is already used.")
                return res.json();
            })
            .catch((error) => {
                this.setState( {error: error})
            })
        this.checkErrors();
    }

    hideResponse = () => {
        const classes = this.responseRef.current.className;
        if(classes.includes('active3'))
            this.responseRef.current.className = classes.replace("active3", "none2");
        else
            this.responseRef.current.className = classes.replace("active2", "none2");
        this.setState({error: ""})
    }

    checkErrors = () => {
        const classes = this.responseRef.current.className;
        if(this.state.error.message !== undefined) {
            this.responseRef.current.className = classes.replace("none2", "active2");
            return true;
        }
        else {
            this.responseRef.current.className = classes.replace("none2", "active3");
            return false;
        }
    }

    render() {
        const {error} = this.state
        let message;
        if(error.message === undefined)
            message = 'Ok';
        else
            message = error.message;
        return(
            <div className={"containerBoth"}>
                <div className={'response2 none2'} ref={this.responseRef}><div className={'container'}>{message}</div></div>
                <div className={"containerRegister"}>
                    <h3 className={'title'}>Register</h3>
                    <input id={"usernameRegister"} className={"poss Email"} placeholder={"Email"} value={this.state.email} onChange={this.handleEmail} type={"text"}/>
                    <input id={"fullName"} className={"poss Name"} placeholder={"Your name"} value={this.state.fullName} onChange={this.handleName} type={"text"}/>
                    <input id={"passRegister"} className={"poss password"} placeholder={"Password"} value={this.state.password} onChange={this.handlePassword} type={"password"}/>
                    <button className={"poss submit"} type={"submit"} onClick={this.createUser}>Submit</button>
                </div>
                <div className={"containerLogIn"}>
                    <h3 className={'title'}>Log in</h3>
                    <input id={"username"} className={"poss Email"} placeholder={"Email"} value={this.state.emailLog} onChange={this.handleEmailLog} type={"text"}/>
                    <input id={"pass"} className={"poss password"} placeholder={"Password"} value={this.state.passwordLog} onChange={this.handlePasswordLog} type={"password"}/>
                    <button className={"poss submit"} type={"submit"} onClick={this.checkLogin}>Submit</button>
                </div>
            </div>
        )
    }

}

export default Accounts