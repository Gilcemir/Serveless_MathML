const express = require('express');
const bodyParser = require('body-parser');
const omml2mathml = require('omml2mathml');
const { DOMParser } = require('xmldom');

const app = express();
const port = 3000;

app.use(bodyParser.json());

function transformEquationToHtml(omml) {
    const parser = new DOMParser();
    const ommlDom = parser.parseFromString(omml, 'application/xml');

    const mathml = omml2mathml(ommlDom);
    mathml.removeAttribute('display');

    return mathml.outerHTML;
}

app.post('/convert', (req, res) => {
    try {
        const omml = req.body.equation;
        const html = transformEquationToHtml(omml);

        res.json({ html });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

app.listen(port, () => {
    console.log(`Servidor rodando na porta ${port}`);
});