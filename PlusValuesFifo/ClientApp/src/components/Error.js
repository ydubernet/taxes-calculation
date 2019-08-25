import React, { Component } from 'react';
import './Error.css';

export default class Error extends Component
{
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div id="error"><br />{this.props.errorMessage}</div>
        );
    }
}