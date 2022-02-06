class Editor extends React.Component {
    constructor(props) {
        super(props);
        this.putTag = this.putTag.bind(this);
    }
    putTag(tagName, classes, href) {
        let selected = document.getSelection();
        if (selected.anchorNode === selected.focusNode && selected.anchorNode.parentElement.id === 'text') {
            let first = Math.min(selected.anchorOffset, selected.focusOffset),
                second = Math.max(selected.anchorOffset, selected.focusOffset);

            let currText = selected.anchorNode.textContent,
                textBefore = currText.substring(0, first),
                textMiddle = currText.substring(first, second),
                textAfter = currText.substring(second);

            let p = document.createElement(tagName);
            if(classes !== undefined || classes === "")
                p.className = classes;
            if (href !== undefined)
                p.setAttribute('href', href);
            p.innerHTML = textMiddle;

            selected.anchorNode.replaceWith(p);

            if (textBefore.length > 0)
                p.parentNode.insertBefore(document.createTextNode(textBefore), p);
            if (textAfter.length > 0)
                p.parentNode.insertBefore(document.createTextNode(textAfter), p.nextSibling);

            document.getElementById('text-input').setAttribute('value', document.getElementById('text').innerHTML);
        }
        
    }
    render() {
        return (
            <div>
                <div className='tool-panel'>
                    <button onClick={() => this.putTag('p')}>Paragraph</button>
                    <button onClick={() => this.putTag('p', 'code')}>Code</button>
                    <button onClick={() => this.putTag('a', "", "google.com")}>Link</button>
                </div>
                <div id='text' dangerouslySetInnerHTML={{ __html: this.props.text.replaceAll(/\s+/g, ' ') }}></div>
            </div>
            )
    }
}
ReactDOM.render(< Editor text={document.getElementById('text-input').value} />, document.getElementById('editor'));