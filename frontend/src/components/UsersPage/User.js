import React, {Component} from 'react'
import './usersStyle.css'
import {Redirect} from 'react-router-dom'

class User extends Component {

    static isAdmin = false;
    constructor(props) {
        super(props);
        this.state = {
            error: null,
            isLoaded: false,
            items:[],
            admin:false,
            userToUpdate:[],
            name:"",
            email:"",
            password:"",
            isAdmin:0
        };
        this.updateRef = new React.createRef();
    }

    getItems = () => {
        const auth = 'Bearer ' + localStorage.getItem("TOKEN");
        const options = {
            method: "GET",
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Authorization': auth
            }
        }
        fetch("http://localhost:8080/api/users", options)
            .then((res) => {
                if(res.status === 401)
                    throw new DOMException("Unauthorized");
                else
                    return res.json();
            })
            .then(
                (result) => {
                    if(result.length > 1) {
                        this.setState({
                            isLoaded: true,
                            items: result,
                            admin: true
                        });
                        User.isAdmin = true;
                    }
                    else {
                        this.setState({
                            isLoaded: true,
                            items:result
                        })
                    }
                },
                (error) => {
                    this.setState({
                        isLoaded: true,
                        error:  error
                    });
                }
            )
    }

    deleteUser = (index) => {
        if(window.confirm("Are you sure?")) {
            const toDelete = this.state.items[index]['id'];
            fetch("http://localhost:8080/api/users/" + toDelete, {
                method: "DELETE",
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'Authorization': 'Bearer ' + localStorage.getItem("TOKEN")
                }
            })
                .then((response) => {
                    if (response.status === 200)
                        this.getUsers()
                })
        }
    }

    handleName = (e) => {
        this.setState({name: e.target.value})
    }

    handleEmail =(e) => {
        this.setState({email: e.target.value});
    }

    handlePassword = (e) => {
        this.setState({password: e.target.value})
    }

    handleAdmin = (e) => {
        this.setState({isAdmin: e.target.value})
    }


    submitUserChanges = () => {
        let email = ""
        let name = ""
        let password = ""
        let isAdmin = 0

        if(this.state.email === "" && this.state.name === "" && this.state.password === "")
            return;
        else {
            if(this.state.email !== "")
                email = this.state.email.toString()
            else
                email = this.state.userToUpdate.email.toString()
            if(this.state.name !== "")
                name = this.state.name.toString()
            else
                name = this.state.userToUpdate.name.toString()
            if(this.state.password !== "")
                password = this.state.password.toString()
            else
                password = this.state.userToUpdate.password.toString()
            if(this.state.isAdmin === 1)
                isAdmin =  this.state.isAdmin
            else
                isAdmin =  this.state.userToUpdate.isAdmin
        }

        const toUpdate = {
            'fullName': name,
            'email': email,
            'password': password,
            'isAdmin': isAdmin
        };

        fetch("http://localhost:8080/api/users/" + this.state.userToUpdate.id, {
            method: "PUT",
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("TOKEN")
            },
            body: JSON.stringify(toUpdate),
        })
            .then(res => res.json())
    }

    updateUser = (index) => {
        this.setState({userToUpdate: this.state.items[index]})
        const currState = this.updateRef.current.className;
        this.updateRef.current.className = currState.replace("none", "active");
    }

    closeUpdateForm = () => {
        const currState = this.updateRef.current.className;
        this.updateRef.current.className = currState.replace("active", "none");
    }
    componentDidMount() {
        this.getItems();
    }

    render() {
        const {error, isLoaded, items } = this.state;
        const deleteUser = this.deleteUser
        const updateUser = this.updateUser

        if(error) {
            if(error.message === "Unauthorized")
                return (<Redirect to={"/register"}/>);
            else
                return (<div className={"center"}><h2>Error: {error.message}</h2></div>)
            }
        else if (!isLoaded)
            return <div className={"center"}><h2>Loading ...</h2></div>
        else {
            if(this.state.admin === true) {
                return (
                    <div className={"center response"}>
                        <h1 className={"pageTitle"}>Users</h1>
                        <ol>
                            {items.map(function (d, idx) {
                                return (
                                    <li className={"userLi"} key={idx}>id: {d.id} full name: {d.fullName} email: {d.email} admin: {d.isAdmin}
                                        <span className={"Butt del"} onClick={() => {updateUser(idx)}}>Update</span>
                                        <span className={"Butt up"} onClick={()=>{deleteUser(idx)}}>Delete</span>
                                    </li>

                                )
                            })}
                        </ol>
                        <div className={"updateForm none"} ref={this.updateRef}>
                            <div className={'updateCenterForm'}>
                                <label>
                                    Name:
                                    <input placeholder={this.state.userToUpdate.fullName} value={this.state.name} onChange={this.handleName}/>
                                </label>
                                <label>
                                    Email:
                                    <input placeholder={this.state.userToUpdate.email} value={this.state.email} onChange={this.handleEmail}/>
                                </label>
                                <label>
                                    Password:
                                    <input placeholder={'New password'} type={"password"} value={this.state.password} onChange={this.handlePassword}/>
                                </label>
                                <label>
                                    Admin:
                                    <input placeholder={this.state.userToUpdate.isAdmin === 1? 'Admin': 'User'} value={this.state.isAdmin} onChange={this.handleAdmin}/>
                                </label>
                                <span className={'Butt buttCenterForm'} onClick={this.submitUserChanges}>Submit</span>
                                <span className={'Butt buttCenterForm'} onClick={this.closeUpdateForm}>Close</span>
                            </div>
                        </div>
                        <button type={"button"} className={'Butt logOut'} onClick={() => {localStorage.removeItem("TOKEN"); User.isAdmin = false; this.props.history.push('/register')}}>Log out</button>
                    </div>
                )
            }
            else {
                return (
                    <div className={"center response"}>
                        <p>id: {this.state.items[0].id} name: {this.state.items[0].fullName} email: {this.state.items[0].email} admin: {this.state.items[0].isAdmin}</p>
                        <button type={"button"} className={'Butt logOut'} onClick={() => {localStorage.removeItem("TOKEN"); this.props.history.push('/register')}}>Log out</button>
                    </div>
                )
            }
        }
    }
}

export default User