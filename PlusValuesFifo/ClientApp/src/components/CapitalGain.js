// Clearly inspired from https://gist.github.com/AshikNesin/e44b1950f6a24cfcd85330ffc1713513

import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { post } from 'axios';
import { saveAs } from 'file-saver';

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
                this.setState({ error: err.message });

                var Error = React.createClass({
                    render: function () {
                        return <div id="errors">{this.props.name}</div>
                    }
                });
                var errorDiv = document.createElement("div");
                React.render(<Error name="coucou" />, errorDiv);
                // grab the container
                var container = document.getElementById("container");
                // and replace the child
                container.replaceChild(errorDiv.querySelector("#destination"), document.getElementById("destination"));
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
            <div>
                <h1>Welcome</h1>

                <p>In order to compute your capital gains, please import a CSV file containing your buy & sell events:</p>

                <form onSubmit={this.onFormSubmit}>
                    <div>
                        <div><label>Your file</label></div>
                        <div><input type="file" onChange={this.onFileChange} /></div>
                    </div>
                    <div>
                        <div><label>Asset type of your file</label></div>
                        <div>
                            <select value={this.state.assetType} onChange={this.onAssetTypeChange}>
                                <option value="Equity">Equity</option>
                                <option value="CryptoCurrency">CryptoCurrency</option>
                            </select>
                        </div>
                    </div>
                    <button type="submit">Compute</button>
                </form>
                <div id="errors"></div>
            </div>
        );
    }
}