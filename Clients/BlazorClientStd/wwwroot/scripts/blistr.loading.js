window.blistr = {
	clicks: [],
	addClick(e) {
		clicks.push(e)
		console.debug({ message: 'clicked', action: e })
	}
}
document.addEventListener("DOMContentLoaded", () => {
	const buttons = document.querySelectorAll('.action');
	buttons.forEach(function (btn) {
		btn.addEventListener('click', buttonClick)
	})

	const inputs = document.querySelectorAll('[data-auto-start="start-blazor"]')
	inputs.forEach(function (input) {
		console.debug({ message: 'Configuring auto-start', input })
		input.addEventListener('keydown', inputStart)
		input.addEventListener('mouseenter', inputStart)
	})
	SunPosition()
	setInterval(SunPosition, 5000)
	setTimeout(StartBlazor('timer'), 1000)
})

function SunPosition() {
	// set css prop --sun-x on the body element from -10% to 110% based on time of day
	// get time of day as a percentage of the whole day
	const now = new Date()
	const hours = now.getHours()
	const minutes = now.getMinutes()
	const seconds = now.getSeconds()
	const totalSeconds = (hours * 60 * 60) + (minutes * 60) + seconds
	const percentOfDay = 160 * totalSeconds / 86400
	const sunX = -30 + percentOfDay
	document.body.style.setProperty('--sun-x', `${sunX}`)
}
async function inputStart(e) {
	const input = e.target
	input.removeEventListener(e.type, inputStart)
	StartBlazor('inputStart')
}
function buttonClick(e) {
	e.preventDefault()
	const btn = e.target
	btn.removeEventListener(e.type, buttonClick)
	window.blistr.addClick({ btn, e })
	StartBlazor('buttonClick')
}

function StartBlazor(caller) {
	console.debug({ message: 'StartBlazor called', caller })
	Blazor
		.start()
 		.then(() => setTimeout(HandleClicks, 128))
		.catch((error) => console.error)
}

function HandleClicks() {
	blistr.clicks.forEach((click) => {
		const newClick = new MouseEvent(click.e.type, click.e)
		var newBtn = document.querySelector(`#${click.btn.id}`)
		if (newBtn && newClick) {
			console.log(newBtn, newClick)
			newBtn.dispatchEvent(newClick)
		}
		else
			console.error({ message: 'Could not find button', click })
	})
	blistr.clicks = []
}
