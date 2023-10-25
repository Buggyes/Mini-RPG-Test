using Godot;
using System;

public partial class battleManager : Node
{
    private int maxPlayerHP, PlayerHP;
    private int maxEnemyHP, EnemyHP;

    private int playerDMG;
    private int enemyDMG;

    private bool playerTurn = true;
    private bool inBattle = true;
    private bool playerIsAlive = true;

    private Panel textBox;
    private Panel actionMenu;
    private Panel gameOverScreen;

    private TextureProgressBar playerHealthBar;
    private TextureProgressBar enemyHealthBar;

    private Label playerHealthText;
    private Label enemyHealthText;
    public override void _Ready()
    {
        base._Ready();

        InitializeBattle();
    }

    public void InitializeBattle()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();

        maxPlayerHP = rand.RandiRange(100, 400);
        maxEnemyHP = rand.RandiRange(100, 350);

        playerDMG = rand.RandiRange(20, 60);
        enemyDMG = rand.RandiRange(25, 50);

        PlayerHP = maxPlayerHP;
        EnemyHP = maxEnemyHP;

        textBox = GetNode<Panel>("./HUD/TextBox");
        actionMenu = GetNode<Panel>("./HUD/Actions");
        gameOverScreen = GetNode<Panel>("./HUD/GameOver");

        UpdateTextBoxLabel(">A wild cube appears");

        playerHealthBar = GetNode<TextureProgressBar>("./Player/HealthBar/Bar");
        enemyHealthBar = GetNode<TextureProgressBar>("./Fighter/HealthBar/Bar");

        playerHealthBar.MaxValue = maxPlayerHP;
        playerHealthBar.Value = maxPlayerHP;

        enemyHealthBar.MaxValue = maxEnemyHP;
        enemyHealthBar.Value = maxEnemyHP;

        playerHealthText = GetNode<Label>("./Player/HealthBar/Bar/Label");
        enemyHealthText = GetNode<Label>("./Fighter/HealthBar/Bar/Label");

        playerHealthText.Text = PlayerHP + "/" + maxPlayerHP;
        enemyHealthText.Text = EnemyHP + "/" + maxEnemyHP;

        HideGameOver();
    }

    public void ResetBattle()
    {
        PlayerHP = maxPlayerHP;
        EnemyHP = maxEnemyHP;

        playerHealthBar.MaxValue = maxPlayerHP;
        playerHealthBar.Value = maxPlayerHP;

        enemyHealthBar.MaxValue = maxEnemyHP;
        enemyHealthBar.Value = maxEnemyHP;

        playerHealthText.Text = PlayerHP + "/" + maxPlayerHP;
        enemyHealthText.Text = EnemyHP + "/" + maxEnemyHP;

        inBattle = true;

        UpdateTextBoxLabel(">A wild cube appears");
        ShowMessage();

        HideGameOver();
    }

    public void CombatCheck()
    {
        if (inBattle)
        {
            if (!playerTurn && actionMenu.Visible)
            {
                EnemyAttack();
            }
        }
    }

    public void PlayerAttack()
    {
        UpdateTextBoxLabel(">The player deals " + playerDMG + " damage to the red cube.");
        ShowMessage();
        EnemyHP = DealDamage(EnemyHP, playerDMG);
        enemyHealthText = UpdateHealthDisplayText(enemyHealthText, EnemyHP, maxEnemyHP);
        enemyHealthBar = UpdateHealthDisplay(enemyHealthBar, EnemyHP);
        playerTurn = false;
    }

    public void EnemyAttack()
    {
        UpdateTextBoxLabel(">The enemy deals " + enemyDMG + " damage to the player.");
        ShowMessage();
        PlayerHP = DealDamage(PlayerHP, enemyDMG);
        playerHealthText = UpdateHealthDisplayText(playerHealthText, PlayerHP, maxPlayerHP);
        playerHealthBar = UpdateHealthDisplay(playerHealthBar, PlayerHP);
        playerTurn = true;
    }

    public void UpdateTextBoxLabel(string text)
    {
        var textBoxLabel = textBox.GetChild<Label>(0);
        textBoxLabel.Text = text;
    }

    public Label UpdateHealthDisplayText(Label display, int currentHP, int maxHP)
    {
        display.Text = currentHP + "/" + maxHP;
        return display;
    }

    public TextureProgressBar UpdateHealthDisplay(TextureProgressBar display, int currentHP)
    {
        display.Value = currentHP;
        return display;
    }

    public int DealDamage(int targetHP, int damage)
    {
        targetHP -= damage;
        if(targetHP < 0)
        {
            targetHP = 0;
        }
        return targetHP;
    }

    public void ShowMessage()
    {
        actionMenu.Visible = false;
        textBox.Visible = true;
    }

    public void HideMessage()
    {
        if (inBattle)
        {
            textBox.Visible = false;
            actionMenu.Visible = true;

            if (PlayerHP <= 0)
            {
                ShowMessage();
                UpdateTextBoxLabel(">The player's vision starts to blur. He loses counciousness as he falls into the ground.");
                inBattle = false;
                playerIsAlive = false;
            }
            else if (EnemyHP <= 0)
            {
                ShowMessage();
                UpdateTextBoxLabel(">The cube breaks down like glass, disapearing after a few moments.");
                inBattle = false;
            }
        }
        else
        {
            textBox.Visible = false;
            if (!playerIsAlive)
            {
                ShowGameOver();
            }
        }
        CombatCheck();
    }

    public void QuitGame()
    {
        GetTree().Quit();
    }
    
    private void HideGameOver()
    {
        gameOverScreen.Visible = false;
    }

    private void ShowGameOver()
    {
        gameOverScreen.Visible = true;
    }
}
