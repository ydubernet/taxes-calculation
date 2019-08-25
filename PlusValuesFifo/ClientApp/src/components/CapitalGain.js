// Clearly inspired from https://gist.github.com/AshikNesin/e44b1950f6a24cfcd85330ffc1713513

import React, { Component } from 'react';
import { post } from 'axios';
import { saveAs } from 'file-saver';
import Error from './Error.js';
import './Error.css';

export class CapitalGain extends Component {

    constructor(props) {
        super(props);
        this.state = {
            file: null,
            assetType: 'Equity'
        }
        this.onFormSubmit = this.onFormSubmit.bind(this)
        this.onFileChange = this.onFileChange.bind(this)
        this.onAssetTypeChange = this.onAssetTypeChange.bind(this)
        this.fileUpload = this.fileUpload.bind(this)
    }
    onFormSubmit(e) {
        e.preventDefault() // Stop form submit
        this.fileUpload(this.state.file, this.state.assetType)
            .then((response) => {
                this.setState({ error: '', msg: 'Successfully uploaded file' });
                var blob = new Blob([response.data], { type: "text/csv;charset=utf-8" });
                saveAs(blob, 'PlusValues.csv');
            }).catch(err => {
                this.setState({ error: "Error : " + err.response.data });
                console.log(err);
            });
    }
    onFileChange(e) {
        this.setState({ file: e.target.files[0] })
    }
    onAssetTypeChange(e) {
        this.setState({ assetType: e.target.value })
    }
    fileUpload(file, assetType) {
        const url = '/api/plusvalues';
        const formData = new FormData();
        formData.append('file', file)
        formData.append('assetType', assetType)
        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        }
        return post(url, formData, config)
    }

    render() {
        return (
            <div id="capitalGain">
                <h1>Welcome</h1>

                <p>In order to compute your capital gains, please import a CSV file containing your buy & sell events:</p>

                <form onSubmit={this.onFormSubmit}>
                    <table>
                        <thead>
                            <th><label>Your file</label></th>
                            <th><label>Asset type of your file</label></th>
                        </thead>
                        <tbody>
                            <td><input type="file" onChange={this.onFileChange} /></td>
                            <td>
                                <select value={this.state.assetType} onChange={this.onAssetTypeChange}>
                                    <option value="Equity">Equity</option>
                                    <option value="CryptoCurrency">Crypto currency</option>
                                </select>
                            </td>
                            <td><button type="submit">Compute</button></td>
                        </tbody>
                    </table>
                </form>
                <Error errorMessage={this.state.error} />
            </div>
        );
    }
}