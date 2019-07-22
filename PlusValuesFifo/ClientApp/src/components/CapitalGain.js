// Clearly inspired from https://gist.github.com/AshikNesin/e44b1950f6a24cfcd85330ffc1713513

import React, { Component } from 'react';
import axios, { post } from 'axios';

export class CapitalGain extends Component {

    constructor(props) {
        super(props);
        this.state = {
            file: null
        }
        this.onFormSubmit = this.onFormSubmit.bind(this)
        this.onChange = this.onChange.bind(this)
        this.fileUpload = this.fileUpload.bind(this)
    }
    onFormSubmit(e) {
        e.preventDefault() // Stop form submit
        this.fileUpload(this.state.file).then((response) => {
            console.log(response.data);
        })
    }
    onChange(e) {
        this.setState({ file: e.target.files[0] })
    }
    fileUpload(file) {
        const url = '/api/plusvalues';
        const formData = new FormData();
        formData.append('file', file)
        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        }
        return post(url, formData, config)
    }

    render() {
        return (
            <div>
                <h1>Welcome</h1>

                <p>In order to compute your capital gains, please import a CSV file containing your buy & sell events:</p>

                <form onSubmit={this.onFormSubmit}>
                    <input type="file" onChange={this.onChange} />
                    <button type="submit">Upload</button>
                </form>
            </div>
        );
    }
}